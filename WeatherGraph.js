import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Line } from 'react-chartjs-2';

const WeatherGraph = () => {
    const [weatherData, setWeatherData] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(''); 
                setWeatherData(response.data);
            } catch (error) {
                console.error('Error fetching weather data:', error);
            }
        };

        fetchData();
    }, []);

    const labels = weatherData.map(data => data.city); 
    const temperatures = weatherData.map(data => data.temperature); 

    const data = {
        labels: labels,
        datasets: [
            {
                label: 'Temperature',
                data: temperatures,
                fill: false,
                borderColor: 'rgba(75, 192, 192, 1)',
                tension: 0.1
            }
        ]
    };

    return (
        <div>
            <h2>Weather Data Graph</h2>
            <Line data={data} />
        </div>
    );
};

export default WeatherGraph;
