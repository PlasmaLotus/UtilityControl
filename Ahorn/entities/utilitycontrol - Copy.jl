module UtilityControl

using ..Ahorn, Maple

@mapdef Entity "utilitycontrol/popblock" PopBlock(x::Integer, y::Integer, width::Integer=defaultBlockWidth, height::Integer=defaultBlockHeight, color::String="Pink")
@mapdef Entity "utilitycontrol/popblockdetonator" PopBlockDetonator(x::Integer, y::Integer, width::Integer=defaultBlockWidth, height::Integer=defaultBlockHeight, color::String="Pink", detonationSpeed::String="Medium", detonationDelay::Integer=defaultDetonationDelay)

placements = Ahorn.PlacementDict()

defaultBlockWidth = 8
defaultBlockHeight = 8
defaultDetonationDelay = 120

popNames = String["utilitycontrol/popblock", "utilitycontrol/popblockdetonator"]
colorString = String["Purple", "Blue", "Red", "Yellow", "Green", "Pink"]
detonationSpeed = String["Slow", "Medium","Fast"]

popBlockDetonationSpeed = Dict{String, Integer}(
    "Slow" => 18,
    "Medium" => 13,
    "Fast" => 8
)

colors = Dict{String, Any}(
    "Purple" => (1.0, 0.3, 1.0, 1.0),
    "Blue" => (0.3, 0.3, 1.0, 1.0),
    "Red" => (1.0, 0.3, 0.3, 1.0),
    "Yellow" => (1.0, 1.0, 0.3, 1.0),
    "Green" => (0.3, 1.0, 0.3, 1.0),
	"Pink" => (1.0, 0.5, 0.8, 1.0)
)

#
#Placements 
#

for (colorName, color) in colors
    placements["PopBlock ($colorName) (UtilityControl)"] = Ahorn.EntityPlacement(
        PopBlock,
        "rectangle",
        Dict{String, Any}(
            "color" => colorName
        )
    )
end


for (colorName, color) in colors
	for (speed, sp) in popBlockDetonationSpeed
		placements["PopBlockDetonator ($colorName, $speed) (UtilityControl)"] = Ahorn.EntityPlacement(
			PopBlockDetonator,
			"point",
			Dict{String, Any}(
				"color" => colorName,
				"detonationSpeed" => speed
			)
		)
    end
end

#
#Editing options
#

Ahorn.editingOptions(entity::PopBlock) = Dict{String, Any}(
	"color" => colorString
)
Ahorn.minimumSize(entity::PopBlock) = 8, 8
Ahorn.resizable(entity::PopBlock) = true, true

function Ahorn.selection(entity::PopBlock)
    x, y = Ahorn.position(entity)
	width = Int(get(entity.data, "width", defaultBlockWidth))
	height = Int(get(entity.data, "height", defaultBlockHeight))
	texture = "objects/utilitycontrol/popblock.png"

	return Ahorn.Rectangle(x, y, width, height)

end

Ahorn.editingOptions(entity::PopBlockDetonator) = Dict{String, Any}(
	"color" => colorString,
	"detonationSpeed" => detonationSpeed,
	"detonationDelay" = get(entity.data, "detonationDelay", defaultDetonationDelay)
)
Ahorn.minimumSize(entity::PopBlockDetonator) = 8, 8

function Ahorn.selection(entity::PopBlockDetonator)
    x, y = Ahorn.position(entity)
	width = get(entity.data, "width", defaultBlockWidth)
	height = get(entity.data, "height", defaultBlockHeight)
	texture = "objects/utilitycontrol/popblockdetonator.png"

	#return Ahorn.Rectangle(x, y, width, height)
	return Ahorn.getSpriteRectangle( texture, x, y, jx=0.5, jy=0.5)
end

#
#Rendering
#

borderMultiplier = (0.9, 0.9, 0.9, 1)
fillMultiplier = (0.9, 0.9, 0.9, 0.4)
##render

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::PopBlockDetonator, room::Maple.Room)
	x=0
	y=0
	alpha = 1
	width = get(entity.data, "width", defaultBlockWidth)
	height = get(entity.data, "height", defaultBlockHeight)
	color = get(entity.data, "color", "Pink")
	texture = "objects/utilitycontrol/popblock.png"
	
	#Ahorn.drawImage(ctx, texture, x, y,  alpha, colors[color])
	
	Ahorn.drawRectangle(ctx, x, y, width, height, colors[color] .* fillMultiplier, colors[color] .* borderMultiplier)
	Ahorn.drawCircle(ctx, (width/2), (height/2), 3.5, colors[color] .* borderMultiplier)
end


##TODO render blocks individually

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::PopBlock, room::Maple.Room)
	x=0
	y=0
	alpha = 1
	width = get(entity.data, "width", defaultBlockWidth)
	height = get(entity.data, "height", defaultBlockHeight)
	texture = "objects/utilitycontrol/popblock.png"
	
	color = get(entity.data, "color", "Pink")
	if haskey(colors, color)
		#Ahorn.drawRectangle(ctx, -8, -8, 16, 16, colors[color], colors[color] .* borderMultiplier)
		#Ahorn.drawRectangle(ctx, -(width/2), -(height/2), width, height, colors[color], colors[color] .* borderMultiplier)
	end
	#Ahorn.drawImage(ctx, texture, x, y,  alpha, colors[color])
	
	#Ahorn.drawRectangle(ctx, 0, 0, width, height, colors[color] .* fillMultiplier, colors[color] .* borderMultiplier)
	Ahorn.drawRectangle(ctx, x, y, width, height, colors[color]*fillMultiplier, colors[color] .* borderMultiplier)
end

end