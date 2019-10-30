module UtilityControl

using ..Ahorn, Maple

@mapdef Entity "utilitycontrol/popblock" PopBlock(x::Integer, y::Integer, width::Integer=defaultBlockWidth, height::Integer=defaultBlockHeight, color::String="Pink")
@mapdef Entity "utilitycontrol/popblockdetonator" PopBlockDetonator(x::Integer, y::Integer, width::Integer=defaultDetonatorWidth, height::Integer=defaultDetonatorHeight, color::String="Pink", detonationSpeed::String="Medium")


placements = Ahorn.PlacementDict()

defaultBlockWidth = 8
defaultBlockHeight = 8
defaultDetonatorWidth = 8
defaultDetonatorHeight = 8
defaultDetonationDelay = 120
borderMultiplier = (0.9, 0.9, 0.9, 0.8)
fillMultiplier = (0.9, 0.9, 0.9, 0.3)
popNames = String["utilitycontrol/popblock", "utilitycontrol/popblockdetonator"]
const colorString = String["Purple", "Blue", "Red", "Yellow", "Green", "Pink"]
const detonationSpeed = String["Slow", "Medium","Fast"]

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
	for (speedName, sp) in popBlockDetonationSpeed
		placements["PopBlockDetonator ($colorName)($speedName) (UC)"] = Ahorn.EntityPlacement(
			PopBlockDetonator,
			"point",
			Dict{String, Any}(
				"color" => colorName, 
				"detonationSpeed" => speedName
			)
		)
	end
end

for (colorName, color) in colors
	placements["PopBlock ($colorName) (UC)"] = Ahorn.EntityPlacement(
		PopBlock,
		"rectangle",
		Dict{String, Any}(
			"color" => colorName, 
		)
	)
end

#
#Editing options
#

Ahorn.editingOptions(entity::PopBlockDetonator) = Dict{String, Any}(
	"color" => colorString,
	"detonation speed" => detonationSpeed
)
Ahorn.minimumSize(entity::PopBlockDetonator) = defaultDetonatorWidth, defaultDetonatorHeight

function Ahorn.selection(entity::PopBlockDetonator)
    x, y = Ahorn.position(entity)
	width = Int(get(entity.data, "width", defaultBlockWidth))
	height = Int(get(entity.data, "height", defaultBlockHeight))
	texture = "objects/utilitycontrol/popblockdetonator.png"

	return Ahorn.getSpriteRectangle(texture, x, y,  jx=0, jy=0)
end


Ahorn.editingOptions(entity::PopBlock) = Dict{String, Any}(
	"color" => colorString
)
Ahorn.minimumSize(entity::PopBlock) = defaultBlockWidth, defaultBlockHeight
Ahorn.resizable(entity::PopBlock) = true, true

function Ahorn.selection(entity::PopBlock)
    x, y = Ahorn.position(entity)
	width = Int(get(entity.data, "width", defaultBlockWidth))
	height = Int(get(entity.data, "height", defaultBlockHeight))
	texture = "objects/utilitycontrol/popblock.png"
	return Ahorn.Rectangle(x, y, width, height)
	#return Ahorn.getEntityRectangle(entity)
	#return Ahorn.getSpriteRectangle(texture, x, y,  jx=0, jy=0)
	#return Ahorn.getSpriteRectangle(texture, x, y, sx=width/defaultBlockWidth, sy=height/defaultDetonatorHeight)

end


#
#Rendering
#

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::PopBlockDetonator)
	x=0
	y=0
	alpha = 1
	width = get(entity.data, "width", defaultBlockWidth)
	height = get(entity.data, "height", defaultBlockHeight)
	color = get(entity.data, "color", "Pink")
	texture = "objects/utilitycontrol/popblockdetonator.png"
	
	if haskey(colors, color)
        Ahorn.drawRectangle(ctx, x, y, width, height, colors[color].* fillMultiplier, colors[color] .* borderMultiplier)
    end
	Ahorn.drawSprite(ctx, texture,  width / 2, height / 2)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::PopBlock)
	x=0
	y=0
	alpha = 1
	width = get(entity.data, "width", defaultBlockWidth)
	height = get(entity.data, "height", defaultBlockHeight)
	color = get(entity.data, "color", "Pink")
	texture = "objects/utilitycontrol/popblock.png"
	
	if haskey(colors, color)
        Ahorn.drawRectangle(ctx, x, y, width, height, colors[color].* fillMultiplier, colors[color] .* borderMultiplier)
    end
	#Ahorn.drawSprite(ctx, texture,  width / 2, height / 2)
	#TODO: draw multiple sprites
end


end