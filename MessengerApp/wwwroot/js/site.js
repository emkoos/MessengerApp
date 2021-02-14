var createRoomBtn = document.getElementById('create-room-btn')
var createRoomModule = document.getElementById('create-room-module')

createRoomBtn.addEventListener('click', function () {
    createRoomModule.classList.add('active')
})

var closeModule = function() {
    createRoomModule.classList.remove('active')
}