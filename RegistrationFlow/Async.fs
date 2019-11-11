module Ploeh.Samples.Async

let map f x = async {
    let! x' = x
    return f x' }

let bind f x = async {
    let! x' = x
    return! f x' }

