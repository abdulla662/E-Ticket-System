﻿document.addEventListener("DOMContentLoaded", function () {
    particlesJS("particles-js", {
        particles: {
            number: { value: 100 },
            size: { value: 3 },
            move: { enable: true, speed: 2 }
        },
        interactivity: {
            detect_on: "canvas",
            events: {
                onhover: { enable: false },
                onclick: { enable: false }
            }
        }
    });

    console.log("Particles.js loaded and adjusted.");
});
particlesJS('particles-js', {
    "particles": {
        "number": {
            "value": 100,
            "density": {
                "enable": true,
                "value_area": 800
            }
        },
        "color": {
            "value": "#ffffff"
        },
        "shape": {
            "type": "circle",
            "stroke": {
                "width": 0,
                "color": "#000000"
            }
        },
        "opacity": {
            "value": 0.5,
            "random": false
        },
        "size": {
            "value": 3,
            "random": true
        },
        "line_linked": {
            "enable": true,
            "distance": 150,
            "color": "#ffffff",
            "opacity": 0.4,
            "width": 1
        },
        "move": {
            "enable": true,
            "speed": 2,
            "direction": "none",
            "random": false,
            "straight": false,
            "out_mode": "out"
        }
    },
    "interactivity": {
        "detect_on": "canvas",
        "events": {
            "onhover": {
                "enable": true,
                "mode": "grab"  // أو استخدم "repulse" إذا كنت تريد أن تبتعد الجسيمات عند التحويم
            },
            "onclick": {
                "enable": true,
                "mode": "push"
            }
        },
        "modes": {
            "grab": {
                "distance": 200,  // كلما زادت المسافة زادت استجابة الجسيمات للمؤشر
                "line_linked": {
                    "opacity": 1
                }
            },
            "repulse": {
                "distance": 100,
                "duration": 0.4
            }
        }
    },
    "retina_detect": true
});
