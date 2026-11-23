# 🚗 3D Car Arena Battle

![Unity](https://img.shields.io/badge/Unity-6-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-OOP-239120?style=for-the-badge&logo=csharp)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows)

**3D Arena Battle: Dynamic Environment Interactive Survival Game** (*3D Arena Savaşı: Dinamik Çevre Etkileşimli Hayatta Kalma Oyunu*) is a 3D vehicular combat and survival game developed in **Unity 6** using **C#** as a Computer Science graduation project at **Dokuz Eylül University**.

Instead of a static map, players battle against AI bots on a constantly shrinking, reactive arena where ground tiles collapse under vehicle weight and regenerate over time.

---

## 🎮 Gameplay & Features

- **Last Car Standing:** Compete against AI bots in an elevated 3D arena and survive as the last vehicle standing.
- **Dynamic Falling Arena:** Every tile reacts to vehicle contact—standing or driving on a tile triggers a countdown before it drops into the void and later respawns.
- **Explosive Hazards:** Driving over arena bombs launches your vehicle into the air, creating unpredictable physics-based moments.
- **Progression & Economy:** Collect gold coins scattered across the arena to unlock new cars from the garage menu.
- **Tuned Vehicle Physics:** Built with realistic driving dynamics, suspension feedback, and optimized Center of Mass calculations for stability at high speeds.

---

## 🕹️ Controls

| Key | Action |
| :--- | :--- |
| **W / A / S / D** or **Arrow Keys** | Drive & Steer |
| **Space** | Jump |

---

## 🛠️ Technical Architecture

- **Engine & Language:** Unity 6, C# (Object-Oriented Programming)
- **Modular Ground System (FSM):** Each arena block is structured as an independent `GameObject` managed by a 4-state Finite State Machine:
  1. **Idle (Normal):** Default stable state.
  2. **Warning (Dangerous):** Triggered upon wheel contact; visual warning & countdown begin.
  3. **Falling (Removed):** Physics constraints are disabled and gravity pulls the tile down.
  4. **Respawning (Resetting):** Tile regenerates after its cooldown completes.
- **Physics & Elimination:** Uses Unity's `WheelCollider` and `Rigidbody` APIs for real-time wheel-to-ground contact detection. Invisible trigger zones positioned beneath the arena automatically detect fallen vehicles and handle eliminations.

---

## 🚀 How to Run

### Option 1: Open in Unity
1. Clone the repository:

```bash
git clone https://github.com/erenonder35/3d-car-arena-battle.git
```

2. Open **Unity Hub**, click **Add > Add project from disk**, and select the cloned folder.
3. Open the project using **Unity 6** and press **Play**.

### Option 2: Playable Build (Windows)
Download the latest playable `.zip` build from the **Releases** section, extract it, and run `My project.exe`.

---

## 👥 Project Team

Developed at **Dokuz Eylül University, Faculty of Science, Department of Computer Science**.

- **Eren Önder**
- **Muhammet Emir Uygun**
- **Harun Yardımcı**
- **Yaman Arıcı**
- **Emre Barış Borak**

**Advisor:** Assoc. Prof. Dr. Resmiye Nasiboğlu
