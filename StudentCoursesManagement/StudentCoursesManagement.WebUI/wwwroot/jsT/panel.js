document.addEventListener('DOMContentLoaded', function () {
    const menuItems = document.querySelectorAll('.menu-item');
    const contentSections = document.querySelectorAll('[id$="Content"]');
    const pageTitle = document.getElementById('pageTitle');
    const profileMenuBtn = document.getElementById('profileMenuBtn');
    const profileDropdown = document.getElementById('profileDropdown');
    const notificationBtn = document.getElementById('notificationBtn');

    document.addEventListener('DOMContentLoaded', function () {
        const menuItems = document.querySelectorAll('.menu-item');
        const contentSections = document.querySelectorAll('[id$="Content"]');
        const pageTitle = document.getElementById('pageTitle');

        menuItems.forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                const targetPage = item.querySelector('a').getAttribute('data-page');

                const pages = {
                    dashboard: "Dashboard",
                    messages: "Mesajlar",
                    analytics: "İstatistikler",
                    files: "Dosyalar",
                    users: "Kullanıcılar",
                    settings: "Ayarlar",
                    courses: "Dersler",
                    reports: "Raporlar"
                };

                menuItems.forEach(i => i.classList.remove('active'));
                item.classList.add('active');

                contentSections.forEach(section => {
                    section.classList.add('hidden');
                    if (section.id === `${targetPage}Content`) {
                        section.classList.remove('hidden');
                        gsap.from(section, {duration: 0.5, opacity: 0, y: 20, ease: "power3.out"});
                        pageTitle.textContent = pages[targetPage];
                    }
                });
            });
        });
    });


    profileMenuBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        profileDropdown.classList.toggle('hidden');
        gsap.from(profileDropdown, {duration: 0.3, opacity: 0, y: 10, ease: "power2.out"});
    });

    document.addEventListener('click', (e) => {
        if (!profileMenuBtn.contains(e.target) && !profileDropdown.contains(e.target)) {
            profileDropdown.classList.add('hidden');
        }
    });

    notificationBtn.addEventListener('click', () => {
        gsap.to(notificationBtn, {duration: 0.1, scale: 1.1, yoyo: true, repeat: 1});
    });

    const commonChartOptions = {
        chart: {
            foreColor: '#ffffff',
            background: 'transparent',
            toolbar: {
                show: false
            },
            animations: {
                enabled: true,
                easing: 'easeinout',
                speed: 800,
                animateGradually: {
                    enabled: true,
                    delay: 150
                },
                dynamicAnimation: {
                    enabled: true,
                    speed: 350
                }
            }
        },
        colors: ['#8b5cf6', '#3b82f6', '#10b981', '#f59e0b', '#ef4444'],
        xaxis: {
            labels: {
                style: {
                    colors: '#ffffff'
                }
            }
        },
        yaxis: {
            labels: {
                style: {
                    colors: '#ffffff'
                }
            }
        },
        legend: {
            labels: {
                colors: '#ffffff'
            }
        },
        tooltip: {
            theme: 'dark',
            style: {
                fontSize: '12px',
                fontFamily: 'Poppins, sans-serif',
                colors: ['#ffffff']
            },
            background: '#1f2937',
            borderColor: '#374151',
            borderWidth: 1,
            marker: {
                show: true
            },
            onDatasetHover: {
                highlightDataSeries: true
            }
        },
        grid: {
            borderColor: '#ffffff20',
            xaxis: {
                lines: {
                    show: true
                }
            },
            yaxis: {
                lines: {
                    show: true
                }
            }
        }
    };

    const userGrowthChart = new ApexCharts(document.querySelector("#userGrowthChart"), {
        ...commonChartOptions,
        series: [{
            name: 'Users',
            data: [31, 40, 28, 51, 42, 109, 100]
        }],
        chart: {
            ...commonChartOptions.chart,
            type: 'area',
            height: 350
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 3
        },
        xaxis: {
            type: 'datetime',
            categories: ["2024-09-19T00:00:00.000Z", "2024-09-19T01:30:00.000Z", "2024-09-19T02:30:00.000Z", "2024-09-19T03:30:00.000Z", "2024-09-19T04:30:00.000Z", "2024-09-19T05:30:00.000Z", "2024-09-19T06:30:00.000Z"]
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            }
        },
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.7,
                opacityTo: 0.3,
                stops: [0, 90, 100]
            }
        }
    });
    userGrowthChart.render();

    const revenueOverviewChart = new ApexCharts(document.querySelector("#revenueOverviewChart"), {
        ...commonChartOptions,
        series: [{
            name: 'Revenue',
            data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
        }],
        chart: {
            ...commonChartOptions.chart,
            type: 'bar',
            height: 350
        },
        plotOptions: {
            bar: {
                borderRadius: 10,
                dataLabels: {
                    position: 'top',
                }
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return "$" + val + "k";
            },
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#ffffff"]
            }
        },
        xaxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
            position: 'bottom'
        },
        yaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            labels: {
                show: true,
                formatter: function (val) {
                    return "$" + val + "k";
                }
            }
        }
    });
    revenueOverviewChart.render();

    const trafficSourcesChart = new ApexCharts(document.querySelector("#trafficSourcesChart"), {
        ...commonChartOptions,
        series: [44, 55, 13, 43, 22],
        chart: {
            ...commonChartOptions.chart,
            type: 'donut',
            height: 350
        },
        labels: ['Search', 'Direct', 'Social', 'Referral', 'Other'],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }],
        plotOptions: {
            pie: {
                donut: {
                    size: '70%'
                }
            }
        }
    });
    trafficSourcesChart.render();

    const userEngagementChart = new ApexCharts(document.querySelector("#userEngagementChart"), {
        ...commonChartOptions,
        series: [{
            name: 'Engagement',
            data: [31, 40, 28, 51, 42, 109, 100]
        }],
        chart: {
            ...commonChartOptions.chart,
            height: 350,
            type: 'line'
        },
        stroke: {
            width: 5,
            curve: 'smooth'
        },
        xaxis: {
            type: 'datetime',
            categories: ["2024-09-19T00:00:00.000Z", "2024-09-19T01:30:00.000Z", "2024-09-19T02:30:00.000Z", "2024-09-19T03:30:00.000Z", "2024-09-19T04:30:00.000Z", "2024-09-19T05:30:00.000Z", "2024-09-19T06:30:00.000Z"]
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            }
        },
        markers: {
            size: 6,
            strokeWidth: 0,
            hover: {
                size: 9
            }
        }
    });
    userEngagementChart.render();

    function updateCharts() {
        const charts = [userGrowthChart, revenueOverviewChart, trafficSourcesChart, userEngagementChart];
        const randomChart = charts[Math.floor(Math.random() * charts.length)];

        if (randomChart === trafficSourcesChart) {
            const newData = Array(5).fill().map(() => Math.floor(Math.random() * 100) + 1);
            randomChart.updateSeries(newData);
        } else {
            const currentSeries = randomChart.w.globals.series[0];
            if (currentSeries && currentSeries.data) {
                const newData = currentSeries.data.map(() => Math.floor(Math.random() * 100) + 1);
                randomChart.updateSeries([{data: newData}]);
            } else {
                console.error('currentSeries or currentSeries.data is undefined');
            }
        }
    }

    setInterval(updateCharts, 5000);

    gsap.from(".stat-card", {
        duration: 0.8,
        y: 20,
        opacity: 0,
        stagger: 0.2,
        ease: "power3.out"
    });

    gsap.from(".chart-card", {
        duration: 1,
        y: 30,
        opacity: 0,
        stagger: 0.3,
        ease: "power3.out",
        delay: 0.5
    });

    const searchInput = document.querySelector('input[type="text"]');
    searchInput.addEventListener('keyup', (e) => {
        if (e.key === 'Enter') {
            const searchTerm = searchInput.value.toLowerCase();
            const allElements = document.querySelectorAll('p, h1, h2, h3, h4, h5, h6, span, td');

            allElements.forEach(element => {
                if (element.textContent.toLowerCase().includes(searchTerm)) {
                    element.style.backgroundColor = 'rgba(66, 153, 225, 0.3)';
                    element.scrollIntoView({behavior: 'smooth', block: 'center'});
                } else {
                    element.style.backgroundColor = '';
                }
            });
        }
    });

    const settingsForm = document.querySelector('#settingsContent form');
    if (settingsForm) {
        settingsForm.addEventListener('submit', (e) => {
            e.preventDefault();
            const formData = new FormData(settingsForm);
            const data = Object.fromEntries(formData);
            console.log('Settings updated:', data);
            alert('Settings updated successfully!');
        });
    }

    const downloadButtons = document.querySelectorAll('#filesContent button');
    downloadButtons.forEach(button => {
        button.addEventListener('click', () => {
            const fileName = button.closest('li').querySelector('p').textContent;
            console.log(`Downloading ${fileName}`);
            alert(`Downloading ${fileName}`);
        });
    });

    const userActionButtons = document.querySelectorAll('#usersContent button');
    userActionButtons.forEach(button => {
        button.addEventListener('click', () => {
            const action = button.classList.contains('text-blue-500') ? 'Edit' : 'Delete';
            const userName = button.closest('tr').querySelector('td').textContent;
            console.log(`${action} user: ${userName}`);
            alert(`${action} user: ${userName}`);
        });
    });

    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetID = this.getAttribute('href');
            if (targetID && targetID !== '#') {
                const targetElement = document.querySelector(targetID);
                if (targetElement) {
                    targetElement.scrollIntoView({
                        behavior: 'smooth'
                    });
                }
            }
        });
    });
});
