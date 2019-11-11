module Ploeh.Samples.Registration

open System

type Mobile = Mobile of int
type ProofId = ProofId of Guid
type Registration = { Mobile : Mobile }
type CompleteRegistrationResult = ProofRequired of ProofId | RegistrationCompleted

let completeRegistrationWorkflow
    (completeRegistration: Registration -> Async<unit>)
    (proof: bool option)
    (registration: Registration)
    : Async<Mobile option> =
    async {
        match proof with
        | None -> return Some registration.Mobile
        | Some isValid ->
            if isValid then
                do! completeRegistration registration
                return None
            else
                return Some registration.Mobile
    }

