

(function ($)
{
    /* "use strict" */

    var dlabChartlist = function ()
    {
        var screenWidth = $(window).width();
        var marketChart = function ()
        {
            var options = {
                series: [{
                    name: 'series1',
                    data: [200, 400, 300, 400, 200, 400]
                }, {
                    name: 'series2',
                    data: [500, 300, 400, 200, 500, 200]
                }],
                chart: {
                    height: 280,
                    type: 'area',
                    toolbar: {
                        show: false
                    }
                },
                colors: ["#6495ED", "#f5a792"],
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'smooth',
                    width: 3,
                    colors: ["var(--primary)", "orange"],
                },
                legend: {
                    show: false
                },
                grid: {
                    show: false,
                    strokeDashArray: 6,
                    borderColor: '#dadada',
                },
                yaxis: {
                    labels: {
                        style: {
                            colors: '#B5B5C3',
                            fontSize: '12px',
                            fontFamily: 'Poppins',
                            fontWeight: 400

                        },
                        formatter: function (value)
                        {
                            return value + "k";
                        }
                    },
                },
                xaxis: {
                    categories: ["Week 01", "Week 02", "Week 03", "Week 04", "Week 05", "Week 06"],
                    labels: {
                        style: {
                            colors: '#B5B5C3',
                            fontSize: '12px',
                            fontFamily: 'Poppins',
                            fontWeight: 400

                        },
                    }
                },
                fill: {
                    type: 'solid',
                    opacity: 0.05
                },
                tooltip: {
                    x: {
                        format: 'dd/MM/yy HH:mm'
                    },
                },
            };

            var chart = new ApexCharts(document.querySelector("#marketChart"), options);
            chart.render();

            jQuery('#dzOldSeries').on('change', function ()
            {
                jQuery(this).toggleClass('disabled');
                chart.toggleSeries('series1');
            });

            jQuery('#dzNewSeries').on('change', function ()
            {
                jQuery(this).toggleClass('disabled');
                chart.toggleSeries('series2');
            });
        }
        
        var overiewChart = function ()
        {
            var options = {
                series: [{
                    name: 'Tests',
                    type: 'column',
                    data: [75, 85, 72, 100, 50, 100, 80, 75, 95, 35, 75, 100]
                }, {
                    name: 'Quizes',
                    type: 'area',
                    data: [44, 65, 55, 75, 45, 55, 40, 60, 75, 45, 50, 42]
                }, {
                    name: 'Assignments',
                    type: 'line',
                    data: [30, 25, 45, 30, 25, 35, 20, 45, 35, 20, 35, 20]
                }],
                chart: {
                    height: 300,
                    type: 'line',
                    stacked: false,
                    toolbar: {
                        show: false,
                    },
                },
                stroke: {
                    width: [0, 1, 1],
                    curve: 'straight',
                    dashArray: [0, 0, 5]
                },
                legend: {
                    fontSize: '13px',
                    fontFamily: 'poppins',
                    labels: {
                        colors: '#888888',
                    }
                },
                plotOptions: {
                    bar: {
                        columnWidth: '18%',
                        borderRadius: 6,
                    }
                },

                fill: {
                    //opacity: [0.1, 0.1, 1],
                    type: 'gradient',
                    gradient: {
                        inverseColors: false,
                        shade: 'light',
                        type: "vertical",
                        /* opacityFrom: 0.85,
                        opacityTo: 0.55, */
                        colorStops: [
                            [
                                {
                                    offset: 0,
                                    color: 'var(--primary)',
                                    opacity: 1
                                },
                                {
                                    offset: 100,
                                    color: 'var(--primary)',
                                    opacity: 1
                                }
                            ],
                            [
                                {
                                    offset: 0,
                                    color: '#3AC977',
                                    opacity: 1
                                },
                                {
                                    offset: 0.4,
                                    color: '#3AC977',
                                    opacity: .15
                                },
                                {
                                    offset: 100,
                                    color: '#3AC977',
                                    opacity: 0
                                }
                            ],
                            [
                                {
                                    offset: 0,
                                    color: '#FF5E5E',
                                    opacity: 1
                                },
                                {
                                    offset: 100,
                                    color: '#FF5E5E',
                                    opacity: 1
                                }
                            ],
                        ],
                        stops: [0, 100, 100, 100]
                    }
                },
                colors: ["var(--primary)", "#3AC977", "#FF5E5E"],
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul',
                    'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
                ],
                markers: {
                    size: 0
                },
                xaxis: {
                    type: 'month',
                    labels: {
                        style: {
                            fontSize: '13px',
                            colors: '#888888',
                        },
                    },

                },
                yaxis: {
                    min: 0,
                    max:100,
                    tickAmount: 4,
                    labels: {
                        style: {
                            fontSize: '13px',
                            colors: '#888888',
                        },
                    },
                },
                tooltip: {
                    shared: true,
                    intersect: false,
                    y: {
                        formatter: function (y)
                        {
                            if (typeof y !== "undefined")
                            {
                                return y.toFixed(0) + " points";
                            }
                            return y;

                        }
                    }
                }
            };

            var chart = new ApexCharts(document.querySelector("#overiewChart"), options);
            chart.render();

            $(".mix-chart-tab .nav-link").on('click', function ()
            {
                var seriesType = $(this).attr('data-series');
                var columnData = [];
                var areaData = [];
                var lineData = [];
                var newLabels = [];

                switch (seriesType)
                {
                    case "week":
                        columnData = [75, 85, 72, 100, 50, 100, 80];
                        areaData = [44, 65, 55, 75, 45, 55, 40];
                        lineData = [30, 25, 45, 30, 25, 35];
                        newLabels = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']


                        break;
                    case "month":
                        columnData = [20, 50, 80, 52, 10, 80, 50, 30, 95, 10, 60, 85];
                        areaData = [40, 25, 85, 45, 85, 25, 95, 65, 45, 45, 20, 12];
                        lineData = [65, 45, 25, 65, 45, 25, 75, 35, 65, 75, 15, 65];
                        newLabels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

                        break;
                    case "year":
                        columnData = [30, 20, 80, 52, 10, 90, 50, 30, 95, 20, 60, 85];
                        areaData = [40, 25, 40, 45, 85, 25, 50, 65, 45, 60, 20, 12];
                        lineData = [65, 45, 30, 65, 45, 25, 75, 40, 65, 50, 15, 65];
                        newLabels = ['2011', '2022', '2023', '2024', '2025', '2026', '2027', '2028', '2029', '2030', '2031', '2032'];
                        break;
                    case "all":
                        columnData = [20, 50, 80, 60, 10, 80, 50, 40, 95, 20, 60, 85];
                        areaData = [40, 25, 30, 45, 85, 25, 95, 65, 50, 45, 20, 12];
                        lineData = [65, 45, 25, 65, 45, 25, 30, 35, 65, 75, 15, 65];
                        break;
                    default:
                        columnData = [75, 80, 72, 100, 50, 100, 80, 30, 95, 35, 75, 100];
                        areaData = [44, 65, 55, 75, 45, 55, 40, 60, 75, 45, 50, 42];
                        lineData = [30, 25, 45, 30, 25, 35, 20, 45, 35, 30, 35, 20];
                }
                chart.updateOptions({
                    labels: newLabels
                });
                chart.updateSeries([
                    {
                        name: "Number of Student",
                        type: 'column',
                        data: columnData


                    }, {
                        name: 'Revenue',
                        type: 'area',
                        data: areaData

                    }, {
                        name: 'Active Teacher',
                        type: 'line',
                        data: lineData

                    }
                ]);
            })

        }

        /* Function ============ */
        return {
            init: function ()
            {
            },
            load: function ()
            {
                marketChart();
                overiewChart();
            },

            resize: function ()
            {
                chartBarRunning();
            }
        }
    }();

    jQuery(window).on('load', function ()
    {
        setTimeout(function ()
        {
            dlabChartlist.load();
        }, 1000);

    });
})(jQuery);