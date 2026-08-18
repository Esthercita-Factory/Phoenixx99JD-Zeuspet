(function () {
    const mintDark = "#397c66";

    function destroyPreviousChart(canvasId) {
        const previousChart = Chart.getChart(canvasId);
        if (previousChart) previousChart.destroy();
    }

    window.renderWeightChart = function (canvasId, labels, data) {
        destroyPreviousChart(canvasId);

        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        new Chart(canvas, {
            type: "line",
            data: {
                labels,
                datasets: [{
                    data,
                    borderColor: mintDark,
                    backgroundColor: "transparent",
                    borderWidth: 2,
                    tension: 0.4,
                    pointRadius: (context) => context.dataIndex === data.length - 1 ? 3 : 0,
                    pointHoverRadius: 4,
                    pointBackgroundColor: mintDark,
                    pointBorderColor: "#ffffff",
                    pointBorderWidth: 2,
                    fill: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false }, tooltip: { enabled: true } },
                scales: {
                    x: { display: true, grid: { display: false }, border: { display: false }, ticks: { color: "#8a9b97", font: { size: 10 } } },
                    y: { display: false, grid: { display: false }, border: { display: false } }
                }
            }
        });
    };

    window.renderRadarChart = function (canvasId, labels, data) {
        destroyPreviousChart(canvasId);

        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        new Chart(canvas, {
            type: "radar",
            data: {
                labels,
                datasets: [{
                    data,
                    borderColor: mintDark,
                    backgroundColor: "rgba(57,124,102,0.15)",
                    borderWidth: 2,
                    pointRadius: 2,
                    pointBackgroundColor: mintDark,
                    pointBorderColor: mintDark
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
                        grid: { display: false },
                        angleLines: { display: false },
                        pointLabels: { color: "#8a9b97", font: { size: 10 } },
                        ticks: { display: false }
                    }
                }
            }
        });
    };
})();
