(function () {
    const mintDark = "#397c66";
    const mintLight = "#8fc9b0";
    const gridColor = "rgba(57,124,102,0.12)";
    const gridColorDark = "rgba(95,184,150,0.15)";

    function destroyPreviousChart(canvasId) {
        const previousChart = Chart.getChart(canvasId);
        if (previousChart) previousChart.destroy();
    }

    function isDark() {
        return document.documentElement.getAttribute("data-theme") === "dark";
    }

    window.renderWeightChart = function (canvasId, labels, data) {
        destroyPreviousChart(canvasId);

        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        const dark = isDark();
        const textColor = dark ? "#8fa5a0" : "#8a9b97";
        const lineColor = dark ? "#6ccba5" : mintDark;

        new Chart(canvas, {
            type: "line",
            data: {
                labels,
                datasets: [{
                    data,
                    borderColor: lineColor,
                    backgroundColor: dark ? "rgba(108,203,165,0.08)" : "rgba(57,124,102,0.06)",
                    borderWidth: 2.5,
                    tension: 0.4,
                    pointRadius: (context) => context.dataIndex === data.length - 1 ? 4 : 0,
                    pointHoverRadius: 5,
                    pointBackgroundColor: lineColor,
                    pointBorderColor: dark ? "#162420" : "#ffffff",
                    pointBorderWidth: 2,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false }, tooltip: { enabled: true } },
                scales: {
                    x: { display: true, grid: { display: false }, border: { display: false }, ticks: { color: textColor, font: { size: 10 } } },
                    y: { display: false, grid: { display: false }, border: { display: false } }
                }
            }
        });
    };

    window.renderRadarChart = function (canvasId, labels, data) {
        destroyPreviousChart(canvasId);

        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        const dark = isDark();
        const accent = dark ? "#6ccba5" : mintDark;
        const fillAlpha = dark ? "0.25" : "0.18";
        const gridCol = dark ? gridColorDark : gridColor;
        const labelColor = dark ? "#8fa5a0" : "#6b857d";

        new Chart(canvas, {
            type: "radar",
            data: {
                labels,
                datasets: [{
                    data,
                    borderColor: accent,
                    backgroundColor: `rgba(${dark ? "108,203,165" : "57,124,102"},${fillAlpha})`,
                    borderWidth: 2.5,
                    pointRadius: 4,
                    pointHoverRadius: 6,
                    pointBackgroundColor: accent,
                    pointBorderColor: dark ? "#162420" : "#ffffff",
                    pointBorderWidth: 2
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false }, tooltip: { enabled: true } },
                scales: {
                    r: {
                        beginAtZero: true,
                        min: 0,
                        max: 10,
                        grid: { color: gridCol, lineWidth: 1 },
                        angleLines: { color: gridCol, lineWidth: 1 },
                        pointLabels: { color: labelColor, font: { size: 11, weight: "600" } },
                        ticks: { display: false }
                    }
                }
            }
        });
    };

    window.applyTheme = function (theme) {
        document.documentElement.setAttribute("data-theme", theme);
        localStorage.setItem("zeuspet-theme", theme);
    };

    window.getSavedTheme = function () {
        return localStorage.getItem("zeuspet-theme") || "light";
    };
})();
