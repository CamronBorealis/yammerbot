namespace YammerBot.FunctionalCore.Test.Unit

open YammerBot.FunctionalCore
open NUnit.Framework
open FsUnit

type FakeDictionaryService() = 
    member this.nounDefs = [
        "Bat \Bat\, n. [Corrupt. from OE. back, backe, balke; cf. Dan. aften-bakke (aften evening), Sw. natt-backa (natt night), Icel. le[eth]r-blaka (le[eth]r leather), Icel. blaka to flutter.] (Zo[\"o]l.) One of the {Chiroptera}, an order of flying mammals, in which the wings are formed by a membrane stretched between the elongated fingers, legs, and tail. The common bats are small and insectivorous. See {Chiroptera} and {Vampire}. [1913 Webster] Silent bats in drowsy clusters cling.    --Goldsmith. [1913 Webster] {Bat tick} (Zo[\"o]l.), a wingless, dipterous insect of the genus {Nycteribia}, parasitic on bats. [1913 Webster] ||";
        "Bat \Bat\, v. t. [imp. &amp; p. p. {Batted} (b[a^]t\"t[e^]d); p. pr. &amp; vb. n. {Batting}.] To strike or hit with a bat or a pole; to cudgel; to beat. --Holland. [1913 Webster]";
        "Bat \Bat\, v. i. To use a bat, as in a game of baseball; when used with a numerical postmodifier it indicates a baseball player's performance (as a decimal) at bat; as, he batted .270 in 1993 (i.e. he got safe hits in 27 percent of his official turns at bat). [1913 Webster +PJC]";
        "Bat \Bat\ (b[a^]t), n. [OE. batte, botte, AS. batt; perhaps fr. the Celtic; cf. Ir. bat, bata, stick, staff; but cf. also F. batte a beater (thing), wooden sword, battre to beat.] [1913 Webster] 1. A large stick; a club; specifically, a piece of wood with one end thicker or broader than the other, used in playing baseball, cricket, etc. [1913 Webster] 2. In badminton, tennis, and similar games, a racket. [Webster 1913 Suppl.] 3. A sheet of cotton used for filling quilts or comfortables; batting. [1913 Webster] 4. A part of a brick with one whole end; a brickbat. [1913 Webster +PJC] 5. (Mining) Shale or bituminous shale. --Kirwan. [1913 Webster] 6. A stroke; a sharp blow. [Colloq. or Slang] [Webster 1913 Suppl.] 7. A stroke of work. [Scot. &amp; Prov. Eng.] [Webster 1913 Suppl.] 8. Rate of motion; speed. [Colloq.] ``A vast host of fowl . . . making at full bat for the North Sea.'' --Pall Mall Mag. [Webster 1913 Suppl.] 9. A spree; a jollification. [Slang, U. S.] [Webster 1913 Suppl.] 10. Manner; rate; condition; state of health. [Scot. &amp; Prov. Eng.] [Webster 1913 Suppl.] {Bat bolt} (Machinery), a bolt barbed or jagged at its butt or tang to make it hold the more firmly. --Knight. [1913 Webster]";
        "Bat \Bat\, v. t. &amp; i. 1. To bate or flutter, as a hawk. [Obs. or Prov. Eng.] [Webster 1913 Suppl.] 2. To wink. [Local, U. S. &amp; Prov Eng.] [Webster 1913 Suppl.]";
        "Bat \Bat\, n. [Siamese.] Same as {Tical}, n., 1. [Webster 1913 Suppl.]"
                                ] |> Seq.ofList
                
    member this.nonNounDefs = Seq.ofList [ "This \This\ ([th][i^]s), pron. & a.; pl. {These} ([th][=e]z). [OE. this, thes, AS. [eth][=e]s, masc., [eth]e['o]s, fem., [eth]is, neut.; akin to OS. these, D. deze, G. dieser, OHG. diser, deser, Icel. [thorn]essi; originally from the definite article + a particle -se, -si; cf. Goth. sai behold. See {The}, {That}, and cf. {These}, {Those}.] 1. As a demonstrative pronoun, this denotes something that is present or near in place or time, or something just mentioned, or that is just about to be mentioned. [1913 Webster] When they heard this, they were pricked in their heart. --Acts ii. 37. [1913 Webster] But know this, that if the good man of the house had known in what watch the thief would come, he would have watched. --Matt. xxiv. 43. [1913 Webster] 2. As an adjective, this has the same demonstrative force as the pronoun, but is followed by a noun; as, this book; this way to town. [1913 Webster] Note: This may be used as opposed or correlative to that, and sometimes as opposed to other or to a second this. See the Note under {That}, 1. [1913 Webster] This way and that wavering sails they bend. --Pope. [1913 Webster] A body of this or that denomination is produced. --Boyle. [1913 Webster] Their judgment in this we may not, and in that we need not, follow. --Hooker. [1913 Webster] Consider the arguments which the author had to write this, or to design the other, before you arraign him. --Dryden. [1913 Webster] Thy crimes . . . soon by this or this will end. --Addison. [1913 Webster] Note: This, like a, every, that, etc., may refer to a number, as of years, persons, etc., taken collectively or as a whole. [1913 Webster] This twenty years have I been with thee.. --Gen. xxxi. 38. [1913 Webster] I have not wept this years; but now My mother comes afresh into my eyes. --Dryden. [1913 Webster]" ]
    interface IDictionaryService with
        member this.GetDefinitions word = 
            match word with
            | "noun" -> this.nounDefs
            | "nonNoun" -> this.nonNounDefs
            | _ -> Seq.empty


[<TestFixture>] 
type ``Given a Dictionary`` ()=
    let fakeService = FakeDictionaryService()
    let dictionary = Dictionary(fakeService) :> IDictionary
    [<Test>] member test.
     ``when I ask if a noun is a noun I should get true`` ()=
           dictionary.IsNoun "noun" |> should be True

    [<Test>] member test.
     ``when I ask if a nonNoun is a noun I should get false`` ()=
           dictionary.IsNoun "nonNoun" |> should be False

    [<Test>] member test.
     ``when I ask for a definition, I should the first definition available`` ()=
           dictionary.GetDefinition "noun" |> should equal (Seq.head fakeService.nounDefs)
