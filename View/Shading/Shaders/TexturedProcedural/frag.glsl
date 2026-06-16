
#version 420 core
           

in vec3 vNormal;
in vec2 vTextureCoordinate;

uniform sampler2D textureSampler;

out vec4 oColor;

layout (std140, binding = 2) uniform Lighting
{
    vec3 fromDirection; float padding;

    vec3 sunColor;
    float sunStrength;

    vec3 shadowColor;
    float ambientStrength;
} light;

vec3 getLightColor(){
    float actualLightLevel = max(0, dot(vNormal, normalize(light.fromDirection)));
    float lightLevel = max(light.ambientStrength, actualLightLevel);

    vec3 sunColor = light.sunColor * light.sunStrength * actualLightLevel;
    float shadowStrength = exp(-0.3 / light.ambientStrength 
        * max(0, actualLightLevel - light.ambientStrength)) * light.ambientStrength;

    vec3 lightColor = sunColor + light.shadowColor * shadowStrength;

    return lightColor;
}

void main(){
    float lightLevel = max(0, dot(vNormal, normalize(vec3(-1, 2, 1.2))));
    
    vec3 color = texture(textureSampler, vTextureCoordinate).rgb;

    oColor = vec4(color * getLightColor(), 1);
}