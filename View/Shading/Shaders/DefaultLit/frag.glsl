
#version 330 core
           

in vec3 vNormal;
in vec2 vTextureCoordinate;
uniform vec4 color;

out vec4 oColor;

void main(){
    vec3 toSun = normalize(vec3(-1, 2, 1.2));
    float lightLevel = max(0.1, dot(vNormal, toSun));
    
    oColor = vec4(color.rgb * lightLevel, color.a);
}