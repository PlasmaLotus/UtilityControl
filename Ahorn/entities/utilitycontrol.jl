module Spikes

using ..Ahorn, Maple

placements = Dict{String, Ahorn.EntityPlacement}()

defaultBlockWidth = 8
defaultBlockHeight = 8
defaultDetonationDelay = 120

PopBlock(x::Integer, y::Integer, width::Integer=defaultBlockWidth, height::Integer=defaultBlockHeight, color::String="Pink") = Maple.Entity("utilitycontrol/popblock", x=x, y=y, color=color)
PopBlockDetonator(x::Integer, y::Integer, width::Integer=defaultBlockWidth, height::Integer=defaultBlockHeight, color::String="Pink", detonationSpeed::String="Medium", detonationDelay::Integer=defaultDetonationDelay) = Maple.Entity("utilitycontrol/popblockdetonator", x=x, y=y, color=color, detonationSpeed=detonationSpeed, detonationDelay=detonationDelay)


#HelperFunctions
#none

placements = Dict{String, Ahorn.EntityPlacement}()

popNames = ["utilitycontrol/popblock", "utilitycontrol/popblockdetonator"]
colorString = String["Purple", "Blue", "Red", "Yellow", "Green", "Pink"]
detonationString = String["Slow", "Medium","Fast"]

popBlockDetonationSpeed = Dict{String, Integer}(
    "Slow" => 20,
    "Medium" => 14,
    "Fast" => 8
)

#removed purple
colors = Dict{String, Any}(
    "Purple" => (1.0, 0.3, 1.0, 1.0),
    "Blue" => (0.3, 0.3, 1.0, 1.0),
    "Red" => (1.0, 0.3, 0.3, 1.0),
    "Yellow" => (1.0, 1.0, 0.3, 1.0),
    "Green" => (0.3, 1.0, 0.3, 1.0),
	"Pink" => (1.0, 0.5, 0.8, 1.0)
)

#Placements
for (colorName, col) in colors
    placements["PopBlock ($colorName)"] = Ahorn.EntityPlacement(
        PopBlock,
        "rectangle",
        Dict{String, Any}(
            "color" => colorName
        )
    )

end
for (speed, sp) in popBlockDetonationSpeed
    for (colorName, col) in colors
		placements["PopBlockDetonator ($speed, $colorName)"] = Ahorn.EntityPlacement(
			PopBlockDetonator,
			"point",
			Dict{String, Any}(
				"color" => colorName,
				"detonationSpeed" => speed
			)
		)
    end
end


function editingOptions(entity::Maple.Entity)
    if entity.name in popNames
		dict = Dict{String, Any}(
            "color" => colorString
        )
		
		if(entity.name == "utilitycontrol/popblockdetonator")
			dict["detonationSpeed"] = detonationString
		end
        return true, dict	

    end
end


function minimumSize(entity::Maple.Entity)
    if entity.name in popNames
        return true, defaultBlockHeight, defaultBlockWidth
    end
end

function resizable(entity::Maple.Entity)
    if entity.name == "utilitycontrol/popblock"
		# canResize, vertical, horiontal
        return true, true, true
    end
end

#Rendering

function selection(entity::Maple.Entity)
    if entity.name in popNames
        nodes = get(entity.data, "nodes", ())
        x, y = Ahorn.entityTranslation(entity)
		width = Int(get(entity.data, "width", defaultBlockWidth))
        height = Int(get(entity.data, "height", defaultBlockHeight))
        
		
		color = get(entity.data, "color", "Pink")

		#res = Ahorn.Rectangle[Ahorn.Rectangle(x - width/2, y - height/2, width, height)]
		res = Ahorn.Rectangle[Ahorn.Rectangle(x , y , width, height)]

        return true, res
    end
end

borderMultiplier = (0.9, 0.9, 0.9, 1)
fillMultiplier = (0.9, 0.9, 0.9, 0.4)
##render

function render(ctx::Ahorn.Cairo.CairoContext, entity::Maple.Entity, room::Maple.Room)
    if entity.name in popNames
	
		width = get(entity.data, "width", defaultBlockWidth)
		height = get(entity.data, "height", defaultBlockHeight)
			
		color = get(entity.data, "color", "Pink")
		if haskey(colors, color)
			#Ahorn.drawRectangle(ctx, -8, -8, 16, 16, colors[color], colors[color] .* borderMultiplier)
			#Ahorn.drawRectangle(ctx, -(width/2), -(height/2), width, height, colors[color], colors[color] .* borderMultiplier)
		end
		
		#Ahorn.drawRectangle(ctx, -(width/2), -(height/2), width, height, colors[color] .* fillMultiplier, colors[color] .* borderMultiplier)
		Ahorn.drawRectangle(ctx, 0, 0, width, height, colors[color] .* fillMultiplier, colors[color] .* borderMultiplier)

		
		if entity.name == "utilitycontrol/popblockdetonator"
			#function drawCircle(ctx::Cairo.CairoContext, x::Number, y::Number, r::Number, c::colorTupleType=(0.0, 0.0, 0.0))
			#Ahorn.drawCircle(ctx, 0, 0, 4, colors[color] .* borderMultiplier)
			Ahorn.drawCircle(ctx, (width/2), (height/2), 3.5, colors[color] .* borderMultiplier)

		end
	return true   
    end
end



end