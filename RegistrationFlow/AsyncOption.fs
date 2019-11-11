module Ploeh.Samples.AsyncOption

let map f x = async {
    match! x with
    | Some x' -> return Some (f x')
    | None -> return None }

let bind f x = async {
    match! x with
    | Some x' -> return! f x'
    | None -> return None }

let traverse f = function
    | Some x -> async {
        let! x' = f x
        return Some x' }
    | None -> async { return None }