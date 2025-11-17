// Game Canvas Setup
const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');
const statusElement = document.getElementById('status');

// Game Variables
const GRAVITY = 0.5;
const JUMP_STRENGTH = -12;
const MOVE_SPEED = 5;

// Player Object
const player = {
    x: 50,
    y: 100,
    width: 30,
    height: 30,
    velocityX: 0,
    velocityY: 0,
    jumping: false,
    color: '#FF6B6B'
};

// Platform definitions
const platforms = [
    // Ground
    { x: 0, y: 570, width: 800, height: 30, color: '#2ECC71' },
    // Platforms
    { x: 150, y: 480, width: 100, height: 20, color: '#8B4513' },
    { x: 300, y: 400, width: 100, height: 20, color: '#8B4513' },
    { x: 450, y: 320, width: 100, height: 20, color: '#8B4513' },
    { x: 250, y: 250, width: 120, height: 20, color: '#8B4513' },
    { x: 450, y: 180, width: 100, height: 20, color: '#8B4513' },
    { x: 600, y: 250, width: 100, height: 20, color: '#8B4513' },
    { x: 650, y: 400, width: 150, height: 20, color: '#8B4513' }
];

// Goal/Flag
const goal = {
    x: 750,
    y: 350,
    width: 20,
    height: 50,
    color: '#FFD700'
};

// Keyboard state
const keys = {};

// Game state
let gameStarted = false;
let gameWon = false;

// Event Listeners
document.addEventListener('keydown', (e) => {
    keys[e.key] = true;
    if (!gameStarted && !gameWon) {
        gameStarted = true;
        statusElement.textContent = 'Go! Reach the flag!';
    }
});

document.addEventListener('keyup', (e) => {
    keys[e.key] = false;
});

// Update player position and physics
function updatePlayer() {
    if (!gameStarted || gameWon) return;

    // Horizontal movement
    if (keys['ArrowLeft'] || keys['a'] || keys['A']) {
        player.velocityX = -MOVE_SPEED;
    } else if (keys['ArrowRight'] || keys['d'] || keys['D']) {
        player.velocityX = MOVE_SPEED;
    } else {
        player.velocityX = 0;
    }

    // Jumping
    if ((keys['ArrowUp'] || keys['w'] || keys['W'] || keys[' ']) && !player.jumping) {
        player.velocityY = JUMP_STRENGTH;
        player.jumping = true;
    }

    // Apply gravity
    player.velocityY += GRAVITY;

    // Update position
    player.x += player.velocityX;
    player.y += player.velocityY;

    // Boundary checking (left and right walls)
    if (player.x < 0) player.x = 0;
    if (player.x + player.width > canvas.width) player.x = canvas.width - player.width;

    // Platform collision detection
    player.jumping = true; // Assume in air unless on platform
    
    platforms.forEach(platform => {
        if (checkCollision(player, platform)) {
            // Bottom collision (landing on platform)
            if (player.velocityY > 0 && player.y + player.height - player.velocityY <= platform.y) {
                player.y = platform.y - player.height;
                player.velocityY = 0;
                player.jumping = false;
            }
            // Top collision (hitting head)
            else if (player.velocityY < 0 && player.y - player.velocityY >= platform.y + platform.height) {
                player.y = platform.y + platform.height;
                player.velocityY = 0;
            }
            // Side collisions
            else if (player.velocityX > 0) {
                player.x = platform.x - player.width;
            } else if (player.velocityX < 0) {
                player.x = platform.x + platform.width;
            }
        }
    });

    // Check if player fell off the bottom
    if (player.y > canvas.height) {
        resetPlayer();
        statusElement.textContent = 'Oops! Try again!';
    }

    // Check win condition
    if (checkCollision(player, goal)) {
        gameWon = true;
        statusElement.textContent = 'You Win! Refresh to play again!';
    }
}

// Check collision between two rectangles
function checkCollision(rect1, rect2) {
    return rect1.x < rect2.x + rect2.width &&
           rect1.x + rect1.width > rect2.x &&
           rect1.y < rect2.y + rect2.height &&
           rect1.y + rect1.height > rect2.y;
}

// Reset player to starting position
function resetPlayer() {
    player.x = 50;
    player.y = 100;
    player.velocityX = 0;
    player.velocityY = 0;
    player.jumping = false;
}

// Draw everything
function draw() {
    // Clear canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Draw sky gradient
    const gradient = ctx.createLinearGradient(0, 0, 0, canvas.height);
    gradient.addColorStop(0, '#87CEEB');
    gradient.addColorStop(1, '#E0F6FF');
    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw platforms
    platforms.forEach(platform => {
        ctx.fillStyle = platform.color;
        ctx.fillRect(platform.x, platform.y, platform.width, platform.height);
        // Add platform border for better visibility
        ctx.strokeStyle = '#654321';
        ctx.lineWidth = 2;
        ctx.strokeRect(platform.x, platform.y, platform.width, platform.height);
    });

    // Draw goal/flag
    ctx.fillStyle = goal.color;
    ctx.fillRect(goal.x, goal.y, goal.width, goal.height);
    // Flag triangle
    ctx.beginPath();
    ctx.moveTo(goal.x, goal.y);
    ctx.lineTo(goal.x + 30, goal.y + 15);
    ctx.lineTo(goal.x, goal.y + 30);
    ctx.closePath();
    ctx.fillStyle = '#FF4500';
    ctx.fill();

    // Draw player
    ctx.fillStyle = player.color;
    ctx.fillRect(player.x, player.y, player.width, player.height);
    // Add player border
    ctx.strokeStyle = '#C0392B';
    ctx.lineWidth = 2;
    ctx.strokeRect(player.x, player.y, player.width, player.height);
    
    // Draw eyes
    ctx.fillStyle = '#FFF';
    ctx.fillRect(player.x + 8, player.y + 8, 6, 6);
    ctx.fillRect(player.x + 16, player.y + 8, 6, 6);
    ctx.fillStyle = '#000';
    ctx.fillRect(player.x + 10, player.y + 10, 3, 3);
    ctx.fillRect(player.x + 18, player.y + 10, 3, 3);
}

// Main game loop
function gameLoop() {
    updatePlayer();
    draw();
    requestAnimationFrame(gameLoop);
}

// Start the game
gameLoop();
