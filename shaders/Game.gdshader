shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, filter_linear;
uniform float split_offset : hint_range(0.0, 1.0) = 0.0;
uniform vec2 split_normal = vec2(1.0, -0.5);
uniform float impact_flash : hint_range(0.0, 1.0) = 0.0;
uniform bool invert_impact = false;
uniform float speed_line_intensity : hint_range(0.0, 1.0) = 1.0;
uniform float bw_cutoff : hint_range(0.0, 1.0) = 0.5;

float hash(float n) {
    return fract(sin(n) * 43758.5453123);
}

void fragment() {
    vec2 norm = normalize(split_normal);
    vec2 tangent = vec2(-norm.y, norm.x);
    vec2 centered_uv = SCREEN_UV - vec2(0.55, 0.45);
    
    float distance_from_split = dot(centered_uv, norm);
    float position_along_split = dot(centered_uv, tangent);
    
    vec2 displacement = norm * split_offset;
    vec2 uv_offset = (distance_from_split > 0.0) ? -displacement : displacement;
    
    vec4 scene_color = texture(screen_texture, SCREEN_UV + uv_offset);
    vec3 final_color = scene_color.rgb;

    if (impact_flash > 0.0) {
        float brightness = dot(scene_color.rgb, vec3(0.299, 0.587, 0.114));
        float bw_threshold = step(bw_cutoff, brightness);
        
        if (invert_impact) {
            bw_threshold = 1.0 - bw_threshold;
        }

        vec3 impact_color = vec3(bw_threshold);
        final_color = mix(scene_color.rgb, impact_color, impact_flash);
    }

    if (split_offset > 0.0) {
        vec3 void_and_line_color = invert_impact ? vec3(0.0) : vec3(1.0);
        
        float dist_from_void_edge = max(0.0, abs(distance_from_split) - split_offset);
        float ray_noise = hash(floor(position_along_split * 250.0));
        float spike_reach = ray_noise * 0.45 * speed_line_intensity;
        
        float edge_spikes = step(dist_from_void_edge, spike_reach * 0.35);
        float outer_rays = step(0.88, ray_noise) * smoothstep(0.0, 0.25, dist_from_void_edge) * speed_line_intensity;
        
        bool in_void = abs(distance_from_split) < split_offset;
        bool in_lines = (edge_spikes > bw_cutoff || outer_rays > bw_cutoff);
        
        if (in_void || in_lines) {
            final_color = void_and_line_color;
        }
    }

    COLOR.rgb = final_color;
    COLOR.a = scene_color.a;
}
