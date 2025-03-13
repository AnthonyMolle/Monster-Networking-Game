EXTERNAL StartCombat()
EXTERNAL ReactivatePlayer(delay)
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
    