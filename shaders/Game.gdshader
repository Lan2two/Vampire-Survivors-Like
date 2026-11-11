shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, filter_linear;
uniform float split_offset : hint_range(0.0, 1.0) = 0.0;
uniform vec2 split_normal = vec2(1.0, -0.5);
uniform float impact_flash : hint_range(0.0, 1.0) = 0.0;
uniform bool invert_impact = false;
uniform float speed_line_intensity : hint_range(0.0, 1.0) = 1.0;

float hash(float n) {
    return fract(sin(n) * 43758.5453123);
}

void fragment() {
    vec2 norm = normalize(split_normal);
    vec2 centered_uv = SCREEN_UV - vec2(0.55, .45);
    
    float distance_from_split = dot(centered_uv, norm);
    
    vec2 displacement = norm * split_offset;
    vec2 uv_offset = (distance_from_split > 0.0) ? -displacement : displacement;
    
    vec4 scene_color = texture(screen_texture, SCREEN_UV + uv_offset);
    vec3 final_color = scene_color.rgb;

    if (impact_flash > 0.0) {
        float brightness = dot(scene_color.rgb, vec3(0.299, 0.587, 0.114));
        float bw_threshold = step(0.5, brightness);
        
        if (invert_impact) {
            bw_threshold = 1.0 - bw_threshold;
        }
    
        float angle = atan(centered_uv.y, centered_uv.x);
        float ray = step(0.92, hash(floor(angle * 350.0)));
        float distance_mask = smoothstep(0.1, 0.45, length(centered_uv));
        float lines = ray * distance_mask * speed_line_intensity;
        
        if (lines > 0.5) {
            bw_threshold = 0.0; 
        }

        vec3 impact_color = vec3(bw_threshold);
        final_color = mix(scene_color.rgb, impact_color, impact_flash);
    }

    if (abs(distance_from_split) < split_offset) {
        final_color = vec3(0.0);
    }

    COLOR.rgb = final_color;
    COLOR.a = scene_color.a;
}
