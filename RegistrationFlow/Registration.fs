module Ploeh.Samples.Registration

open System

type Mobile = Mobile of int
type ProofId = ProofId of Guid
type Registration = { Mobile : Mobile }
type CompleteRegistrationResult = ProofRequired of ProofId | RegistrationCompleted

let completeRegistrationWorkflow
    (createProof: Mobile -> Async<ProofId>)
    (completeRegistration: Registration -> Async<unit>)
    (proof: bool option)
    (registration: Registration)
    : Async<CompleteRegistrationResult> =
    async {
        match proof with
        | None ->
            let! proofId = createProof registration.Mobile
            return ProofRequired proofId
        | Some isValid ->
            if isValid then
                do! completeRegistration registration
                return RegistrationCompleted
            else
                let! proofId = createProof registration.Mobile
                return ProofRequired proofId
    }

