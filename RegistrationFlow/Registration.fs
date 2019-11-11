module Ploeh.Samples.Registration

open System

type Mobile = Mobile of int
type ProofId = ProofId of Guid
type Registration = { Mobile : Mobile }
type CompleteRegistrationResult = ProofRequired of ProofId | RegistrationCompleted

let completeRegistrationWorkflow
    (proof: bool option)
    (registration: Registration)
    : Result<Registration, Mobile> =
    match proof with
    | None -> Error registration.Mobile
    | Some isValid ->
        if isValid then Ok registration
        else Error registration.Mobile
