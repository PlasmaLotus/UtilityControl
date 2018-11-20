module UtilityControl

using ..Ahorn, Maple


popBlockDetonationSpeed = Dict{String, Integer}(
    "Slow" => 20,
    "Medium" => 14,
    "Fast" => 8
)

colors = Dict{String, Any}(
    "Purple" => (1.0, 0.3, 1.0, 1.0),
    "Blue" => (0.3, 0.3, 1.0, 1.0),
    "Red" => (1.0, 0.3, 0.3, 1.0),
    "Yellow" => (1.0, 1.0, 0.3, 1.0),
    "Green" => (0.3, 1.0, 0.3, 1.0)
)

rotations = Dict{Number, Number}(
    1 => 0,
    2 => pi,
    3 => pi * 3 / 2,
    4 => pi / 2
)