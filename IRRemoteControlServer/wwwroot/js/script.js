var tv_open = document.getElementsByClassName("tv_open")[0]
tv_open.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=tv_open").then(function(response){
        console.log("send success")
    })
})

var speaker_open = document.getElementsByClassName("speaker_open")[0]
speaker_open.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=speaker_open").then(function(response){
        console.log("send success")
    })
})

var tv_change_input = document.getElementsByClassName("tv_change_input")[0]
tv_change_input.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=tv_change_input").then(function(response){
        console.log("send success")
    })
})

var tv_enter = document.getElementsByClassName("tv_enter")[0]
tv_enter.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=tv_enter").then(function(response){
        console.log("send success")
    })
})

var volume_up = document.getElementsByClassName("volume_up")[0]
volume_up.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=volume_up").then(function(response){
        console.log("send success")
    })
})

var volume_down = document.getElementsByClassName("volume_down")[0]
volume_down.addEventListener("pointerdown", function(){
    fetch("api/IRRemote/Control?operations=volume_down").then(function(response){
        console.log("send success")
    })
})