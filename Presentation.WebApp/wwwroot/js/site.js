
// ------ For Header menu icon. ( Phone and Tablet ) ------

/*
 * I tried to build the same JavaScript as in the W3School tutorial "How To - Mobile Navigation Menu", but it didn't work as i wanted.
 * So i got help from ChatGPT on how to build the code. ( The W3School example did not use "mosemove" EventListener ).
*/

const topnav = document.querySelector('.topnav');
const myLinks = document.getElementById('my-links')

function myFunction() {
    document.getElementById("my-links").classList.toggle("show");
};

// ---------------------------------------------------



// ---- For the drop-down menu on the "Training-link" (Phone and Tablet).

const navDropdown2 = document.querySelector('.nav-dropdown2');
const myLinks3 = document.getElementById('my-links3');

function myFunction3() {
    document.getElementById("my-links3").classList.toggle('show');
};

// ---------------------------------------------------



// ---- For the drop-down menu on the "Training-link" (Desktop).

const navDropdown = document.querySelector('.nav-dropdown');
const myLinks2 = document.getElementById('my-links2');

function myFunction2() {
    document.getElementById("my-links2").classList.toggle('show');
};

// ---------------------------------------------------



// ----------------- Accordion -----------------------

const items = document.querySelectorAll('.item');

items.forEach(item => {
    item.addEventListener('click', () => {
        const active = document.querySelector('.item.active');

        if (active === item) {
            item.classList.remove('active');
        }
        else {
            active?.classList.remove('active');
            item.classList.add('active');
        }
    });
});

// ---------------------------------------------------