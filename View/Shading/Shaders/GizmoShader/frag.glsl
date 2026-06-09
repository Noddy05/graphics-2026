
#version 330 core
           
uniform vec4 color;

out vec4 oColor;

void main(){
    oColor = color;
    gl_FragDepth = 0;
}