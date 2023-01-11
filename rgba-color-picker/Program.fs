open ImGuiNET.FSharp
open Elmish
open ImGuiNET
open System.Numerics

type Model = {
    Red: float32
    Green: float32
    Blue: float32
    Alpha: float32
    Color: Vector4
    Output: string
}

type Msg = 
    | RedSliderMoved of float32
    | GreenSliderMoved of float32
    | BlueSliderMoved of float32
    | AlphaSliderMoved of float32
    | ColorChanged of Vector4

let init() = {
    Red = 0f
    Green = 0f
    Blue = 0f
    Alpha = 0f
    Color = Vector4(0f,0f,0f,0f)
    Output = ""
}

let SetColor model =
    {model with Color = Vector4(model.Red, model.Green, model.Blue, model.Alpha)}

let setSliders model =
    let (r, g, b, a) = (model.Color.X, model.Color.Y, model.Color.Z, model.Color.W)
    {model with Red = r; Green = g; Blue = b; Alpha = a}

let setOutput model = 
    {model with Output = $"Vector4({model.Red}f, {model.Green}f, {model.Blue}f, {model.Alpha}f)"}

let update (msg: Msg) (model: Model) : Model =
    match msg with 
    | RedSliderMoved a -> setOutput (SetColor { model with Red = a})
    | GreenSliderMoved a -> setOutput (SetColor { model with Green = a})
    | BlueSliderMoved a -> setOutput (SetColor { model with Blue = a})
    | AlphaSliderMoved a -> setOutput (SetColor { model with Alpha = a})
    | ColorChanged a -> setOutput (setSliders { model with Color = a})

let view (model:Model) (dispatch:Msg -> unit) =     
    
    let flags = 
        ImGuiWindowFlags.NoMove +
        ImGuiWindowFlags.NoCollapse +
        ImGuiWindowFlags.NoTitleBar +
        ImGuiWindowFlags.NoSavedSettings

    let gui =
        Gui.app [
            Gui.window "Color picker" flags [
                Gui.sliderFloat 
                    "##slider-red" 
                    (ref model.Red) 
                    0f 
                    1f 
                    "%f" 
                    ImGuiSliderFlags.None 
                    (fun a -> RedSliderMoved(a) |> dispatch) 
                    (fun a -> AlphaSliderMoved(a) |> dispatch)
                Gui.sliderFloat 
                    "##slider-green" 
                    (ref model.Green) 
                    0f 
                    1f 
                    "%f" 
                    ImGuiSliderFlags.None 
                    (fun a -> GreenSliderMoved(a) |> dispatch) 
                    (fun a -> AlphaSliderMoved(a) |> dispatch)
                Gui.sliderFloat 
                    "##slider-blue" 
                    (ref model.Blue) 
                    0f 
                    1f 
                    "%f" 
                    ImGuiSliderFlags.None 
                    (fun a -> BlueSliderMoved(a) |> dispatch) 
                    (fun a -> AlphaSliderMoved(a) |> dispatch)
                Gui.sliderFloat 
                    "##slider-alpha" 
                    (ref model.Alpha) 
                    0f 
                    1f 
                    "%f" 
                    ImGuiSliderFlags.None 
                    (fun a -> AlphaSliderMoved(a) |> dispatch) 
                    (fun a -> AlphaSliderMoved(a) |> dispatch)
                Gui.ColorPicker 
                    "##color-picker" 
                    (ref model.Color) 
                    ImGuiColorEditFlags.None 
                    (fun a-> ColorChanged(a) |> dispatch)
                Gui.InputText 
                    "##output" 
                    (ref model.Output) 
                    (fun a -> ()) 
                    (fun a -> ())
            ]
        ]

    startOrUpdateGuiWith "Color picker" gui |> ignore
    resizeGui( 650, 605)

Program.mkSimple init update view
|> Program.run