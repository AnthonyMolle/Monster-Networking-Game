EXTERNAL StartCombat()
EXTERNAL ShowName()
EXTERNAL ShowPlayerName()
EXTERNAL ShowMysteryName()
EXTERNAL ReactivatePlayer(delay)
EXTERNAL ActivateInternalDialogue(index)
-> main

===main===
oh whoops! dropped it, haha, that's ok
could i get another card?
    + [(run)]
        ~ ReactivatePlayer(true)
        -> DONE
    + [(try again)]
        ~ StartCombat()
        -> DONE
    