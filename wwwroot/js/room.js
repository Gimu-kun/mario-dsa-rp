const token = sessionStorage.getItem("jwtToken"); // hoặc lấy từ Session .NET

// Kết nối SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/roomhub", {
        accessTokenFactory: () => token
    })
    .build();

connection.start().then(() => console.log("✅ SignalR connected"));

// Khi user join phòng -> join luôn vào group SignalR
await connection.invoke("JoinRoomGroup", roomId);

// Lắng nghe khi có người join
connection.on("PlayerJoined", (player) => {
    console.log("Người chơi mới:", player);
    addUserToList(player.Username, player.UserId);
});


// Tạo phòng
async function createRoom(userId, mode) {
    const res = await fetch("/api/room/create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userId, mode })
    });
    const data = await res.json();

    const roomId = data.roomId;
    console.log("Phòng tạo:", roomId);

    // Join realtime group
    connection.invoke("JoinRoom", roomId, userId, "HostPlayer");

    document.getElementById("roomId").textContent = roomId;
}

// Tham gia phòng
async function joinRoom(roomId, userId, username) {
    const res = await fetch("/api/room/join", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ roomId, userId, username })
    });

    const data = await res.json();
    console.log("Tham gia:", data);

    connection.invoke("JoinRoom", roomId, userId, username);
}
