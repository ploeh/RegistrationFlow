module Ploeh.Samples.RegistrationFacadeTests

open System
open Xunit
open Swensen.Unquote
open Ploeh.Samples.Tests.Fakes
open Ploeh.Samples.Registration

let createFixture () =
    let twoFA = Fake2FA ()
    let db = FakeRegistrationDB ()
    let sut pid r = async {
        let! p = AsyncOption.traverse (twoFA.VerifyProof r.Mobile) pid
        return!
            completeRegistrationWorkflow p r
            |> AsyncResult.traverseBoth db.CompleteRegistration twoFA.CreateProof
            |> AsyncResult.cata (fun () -> RegistrationCompleted) ProofRequired
        }
    sut, twoFA, db

[<Theory>]
[<InlineData 123>]
[<InlineData 432>]
let ``Missing proof ID`` mobile = async {
    let sut, twoFA, db = createFixture ()
    let r = { Mobile = Mobile mobile }

    let! actual = sut None r

    let! expectedProofId = twoFA.CreateProof r.Mobile
    let expected = ProofRequired expectedProofId
    expected =! actual
    test <@ Seq.isEmpty db @> }

[<Theory>]
[<InlineData 987>]
[<InlineData 247>]
let ``Valid proof ID`` mobile = async {
    let sut, twoFA, db = createFixture ()
    let r = { Mobile = Mobile mobile }
    let! p = twoFA.CreateProof r.Mobile
    twoFA.VerifyMobile r.Mobile

    let! actual = sut (Some p) r

    RegistrationCompleted =! actual
    test <@ Seq.contains r db @> }

[<Theory>]
[<InlineData 327>]
[<InlineData 666>]
let ``Invalid proof ID`` mobile = async {
    let sut, twoFA, db = createFixture ()
    let r = { Mobile = Mobile mobile }
    let! p = twoFA.CreateProof r.Mobile

    let! actual = sut (Some p) r

    let! expectedProofId = twoFA.CreateProof r.Mobile
    let expected = ProofRequired expectedProofId
    expected =! actual
    test <@ Seq.isEmpty db @> }