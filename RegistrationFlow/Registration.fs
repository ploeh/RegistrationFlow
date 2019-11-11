module Ploeh.Samples.Registration

open System

type Mobile = Mobile of int
type ProofId = ProofId of Guid
type Registration = { Mobile : Mobile }
type CompleteRegistrationResult = ProofRequired of ProofId | RegistrationCompleted

let completeRegistrationWorkflow
    (createProof: Mobile -> Async<ProofId>)
    (verifyProof: Mobile -> ProofId -> Async<bool>)
    (completeRegistration: Registration -> Async<unit>)
    (proofId: ProofId option)
    (registration: Registration)
    : Async<CompleteRegistrationResult> =
    async {
        match proofId with
        | None ->
            let! proofId = createProof registration.Mobile
            return ProofRequired proofId
        | Some proofId ->
            let! isValid = verifyProof registration.Mobile proofId
            if isValid then
                do! completeRegistration registration
                return RegistrationCompleted
            else
                let! proofId = createProof registration.Mobile
                return ProofRequired proofId
    }

