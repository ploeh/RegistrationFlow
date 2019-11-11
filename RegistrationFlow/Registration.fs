module Ploeh.Samples.Registration

open System

type Mobile = Mobile of int
type ProofId = ProofId of Guid
type Registration = { Mobile : Mobile }
type CompleteRegistrationResult = ProofRequired of ProofId | RegistrationCompleted

let completeRegistrationWorkflow
    (proof: bool option)
    (registration: Registration)
    : Async<Result<Registration, Mobile>> =
    async {
        match proof with
        | None -> return Error registration.Mobile
        | Some isValid ->
            if isValid then
                return Ok registration
            else
                return Error registration.Mobile
    }
