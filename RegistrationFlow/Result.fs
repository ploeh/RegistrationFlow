module Ploeh.Samples.Result

let cata f g = function
    | Ok x -> f x
    | Error e -> g e