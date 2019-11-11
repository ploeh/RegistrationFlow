module Ploeh.Samples.Tests.Fakes

open System
open Ploeh.Samples.Registration
open System.Collections.ObjectModel

type Fake2FA () =
    let mutable proofs = Map.empty

    member _.CreateProof mobile =
        match Map.tryFind mobile proofs with
        | Some (proofId, _) -> proofId
        | None ->
            let proofId = ProofId (Guid.NewGuid ())
            proofs <- Map.add mobile (proofId, false) proofs
            proofId
        |> fun proofId -> async { return proofId }

    member _.VerifyProof mobile proofId =
        match Map.tryFind mobile proofs with
        | Some (_, true) -> true
        | _ -> false
        |> fun b -> async { return b }

    member _.VerifyMobile mobile =
        match Map.tryFind mobile proofs with
        | Some (proofId, _) ->
            proofs <- Map.add mobile (proofId, true) proofs
        | _ -> ()

type FakeRegistrationDB () =
    inherit Collection<Registration> ()

    member this.CompleteRegistration r = async { this.Add r }