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
    | Some true -> Ok registration
    | _ -> Error registration.Mobile
