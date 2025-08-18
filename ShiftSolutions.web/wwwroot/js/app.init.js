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

//mark as paid
(function () {
    const openBtn = document.getElementById('markAsPaidBtn');
    const overlay = document.getElementById('payOverlay');
    const closeBtn = document.getElementById('payClose');
    const cancelBtn = document.getElementById('payCancel');
    const confirmBtn = document.getElementById('payConfirm');

    const payDate = document.getElementById('payDate');
    const payTime = document.getElementById('payTime');
    const payMethod = document.getElementById('payMethod');
    const payRef = document.getElementById('payRef');
    const payAmount = document.getElementById('payAmount');
    const payReceipt = document.getElementById('payReceipt');
    const payNotes = document.getElementById('payNotes');
    const payNotify = document.getElementById('payNotify');

    function openM() {
        // default to now
        const now = new Date();
        payDate.value = now.toISOString().slice(0, 10);
        payTime.value = now.toTimeString().slice(0, 5);
        overlay.classList.add('show');
        document.body.style.overflow = 'hidden';
    }
    function closeM() {
        overlay.classList.remove('show');
        document.body.style.overflow = '';
    }

    openBtn?.addEventListener('click', openM);
    closeBtn?.addEventListener('click', closeM);
    cancelBtn?.addEventListener('click', closeM);
    overlay.addEventListener('click', (e) => { if (e.target === overlay) closeM(); });
    document.addEventListener('keydown', (e) => { if (e.key === 'Escape' && overlay.classList.contains('show')) closeM(); });

    function naira(n) { return '₦' + Number(n || 0).toLocaleString('en-NG'); }

    confirmBtn.addEventListener('click', () => {
        // Basic validation
        if (!payDate.value || !payTime.value || !payMethod.value || !payRef.value.trim() || !payAmount.value) {
            alert('Please fill all required fields.');
            return;
        }
        const amount = Number(payAmount.value);
        if (isNaN(amount) || amount <= 0) {
            alert('Enter a valid Amount Paid.');
            return;
        }

        // Build payload for your API
        const payload = {
            batchId: /* put your current batch id here */ null,
            paidAt: `${payDate.value}T${payTime.value}:00`,
            method: payMethod.value,
            reference: payRef.value.trim(),
            amount: amount,
            notes: payNotes.value.trim(),
            notifyMerchants: payNotify.checked,
            // File upload will be handled via multipart/form-data when wiring backend
            receiptFileName: payReceipt.files[0]?.name || null
        };

        console.log('Mark as Paid payload:', payload);

        // Demo feedback
        confirmBtn.disabled = true;
        const old = confirmBtn.textContent;
        confirmBtn.textContent = 'Saving…';
        setTimeout(() => {
            confirmBtn.disabled = false;
            confirmBtn.textContent = old;
            closeM();
            alert(`Batch marked as paid (demo): ${naira(amount)} via ${payload.method}`);
            // TODO: Update UI: replace status chip to "Paid", set paid date/reference on page, etc.
        }, 900);
    });
})();