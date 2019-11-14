module Ploeh.Samples.AsyncResult

let traverseBoth f g = function
    | Ok x -> async {
        let! x' = f x
        return Ok x' }
    | Error e -> async {
        let! e' = g e
        return Error e' }
