namespace YammerBot.FunctionalCore

open System.Text.RegularExpressions                                                                                                     

module WordProcessor = 
    let IsNoun defs = 
        let regex = Regex("^.*, n\.")
        defs
        |> Seq.exists (fun w -> regex.IsMatch w)

    let Sexify (phrase : string) (defs : Map<string,seq<string>>) =
        phrase.Trim().Split(' ')
        |> Seq.map (fun w -> match defs.TryFind(w) with
                             | Some x -> if (IsNoun x) then ("sexy " + w) else w
                             | None -> w)
        |> (fun (coll : seq<string>) -> Seq.fold (fun accum w -> accum + " " + w) (Seq.head coll) (Seq.skip 1 coll))