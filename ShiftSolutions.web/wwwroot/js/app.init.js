document.addEventListener("DOMContentLoaded", () => {
    // Feather icons
    if (window.feather) feather.replace();

    const body = document.body;
    const sidebar = document.getElementById("appSidebar");
    const hamburger = document.getElementById("hamburger");
    const collapseBtn = document.getElementById("sidebarToggle");

    const toggle = () => body.classList.toggle("sidebar-collapsed");

    if (hamburger) hamburger.addEventListener("click", toggle);
    if (collapseBtn) collapseBtn.addEventListener("click", toggle);
});

setTimeout(() => {
    const loader = document.getElementById('pageLoader');
    if (loader) {
        loader.style.opacity = '0';
        loader.style.transition = 'opacity 0.4s ease';
        setTimeout(() => loader.style.display = 'none', 400);
    }
}, 4000);

(function () {
    const wrappers = document.querySelectorAll('[data-dropdown]');

    function closeAll(except) {
        wrappers.forEach(w => {
            if (w !== except) {
                const menu = w.querySelector('[data-dropdown-menu]');
                const btn = w.querySelector('[data-dropdown-button]');
                menu?.classList.add('hidden');
                if (btn) btn.setAttribute('aria-expanded', 'false');
            }
        });
    }

    wrappers.forEach(w => {
        const btn = w.querySelector('[data-dropdown-button]');
        const menu = w.querySelector('[data-dropdown-menu]');
        if (!btn || !menu) return;

        btn.addEventListener('click', (e) => {
            e.stopPropagation();

            // If THIS menu is already open, close it
            if (!menu.classList.contains('hidden')) {
                menu.classList.add('hidden');
                btn.setAttribute('aria-expanded', 'false');
                return;
            }

            // Otherwise, close others and open this one
            closeAll(w);
            menu.classList.remove('hidden');
            menu.classList.add('flex')
            btn.setAttribute('aria-expanded', 'true');
        });
    });
})();

//icons toggle

document.addEventListener('DOMContentLoaded', () => {
  const containers = document.querySelectorAll('.icon-dropdown-container');

    function closeAll() {
        containers.forEach(c => c.querySelector('.icon-dropdown').classList.add('hidden'));
  }

  containers.forEach(c => {
    const btn = c.querySelector('[data-icon-button]');
    const menu = c.querySelector('.icon-dropdown');

    btn.addEventListener('click', e => {
        e.stopPropagation();
    const isOpen = !menu.classList.contains('hidden');
    closeAll();
    if (!isOpen) menu.classList.remove('hidden');
    });
  });

    document.addEventListener('click', closeAll);
});

