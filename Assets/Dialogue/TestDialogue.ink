EXTERNAL StartCombat()
-> main

===main===
this is the main dialogue
do you want to start combat?
    + [yes]
        bet!
        ~ StartCombat()
        -> DONE
    + [no]
        too bad!
        ~ StartCombat()
        -> DONE