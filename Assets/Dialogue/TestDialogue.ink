EXTERNAL StartCombat()
EXTERNAL ShowName(name)
-> main

===main===
this is the main dialogue
my name jacob
~ ShowName("Jacob")
do you want to start combat?
    + [yes]
        bet!
        ~ StartCombat()
        -> DONE
    + [no]
        too bad!
        ~ StartCombat()
        -> DONE