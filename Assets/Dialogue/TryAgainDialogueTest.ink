EXTERNAL StartCombat()
EXTERNAL ReactivatePlayer(delay)
-> main

===main===
you can try again!
do you want to start combat?
    + [yes]
        bet!
        ~ StartCombat()
        -> DONE
    + [no]
        another time....
        ~ ReactivatePlayer(true)
        -> DONE