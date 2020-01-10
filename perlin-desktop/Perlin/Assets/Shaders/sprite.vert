#version 450

layout (set = 0, binding = 0) uniform OrthographicProjection
{
    mat4 Projection;
};

layout(location = 0) in vec2 Pos;
layout(location = 1) in vec2 Size;
layout(location = 2) in float Alpha;
layout(location = 3) in float Rotation;

layout(location = 0) out vec2 fsin_TexCoords;
layout(location = 1) out float fsin_Alpha;

const vec4 Quads[4]= vec4[4]( // pivot is top left by default
    vec4(0, 1, 0, 0), // x, y, textureX, textureY
    vec4(1, 1, 1, 0),
    vec4(0, 0, 0, 1),
    vec4(1, 0, 1, 1)
);
vec2 rotate(vec2 pos, float rot) {
    float s = sin(rot);
    float c = cos(rot);
    mat2 m = mat2(c, -s, s, c); // 2x2 floating point matrix
    return m * pos;
}

void main() {
    vec4 src = Quads[gl_VertexIndex];
    vec2 quadPos = src.xy; 
    quadPos.x = (quadPos.x * Size.x);
    quadPos.y = (quadPos.y * Size.y);
    quadPos = rotate(quadPos, -Rotation);
    quadPos.x = quadPos.x + Pos.x;
    quadPos.y = quadPos.y + Pos.y;
    
    gl_Position = Projection * vec4(quadPos, 0, 1);

    fsin_TexCoords = src.zw;
    fsin_TexCoords.y = -fsin_TexCoords.y;
    fsin_Alpha = Alpha;
}