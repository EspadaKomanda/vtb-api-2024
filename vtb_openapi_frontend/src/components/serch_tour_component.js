"use client";
import { useEffect, useState } from 'react';

class FilterData {
    constructor() {
        this.search = '';
        this.dateFrom = null;
        this.dateTo = null;
        this.priceFrom = 0;
        this.priceTo = 0;
        this.distanceFrom = 0;
        this.distanceTo = 0;
        this.types = [];
        this.ratingFrom = 1;
        this.credit = false;
    }
}

export default function SearchTourComponent() {
    const [isFilterVisible, setIsFilterVisible] = useState(false);
    const [filters, setFilters] = useState(new FilterData());
    const [tours, setTours] = useState([]);

    useEffect(() => {
        const fetchTours = async () => {
            try {
                const response = await fetch('/tours.json'); // Путь к вашему JSON-файлу
                if (!response.ok) {
                    throw new Error('Сеть не отвечает');
                }
                const data = await response.json();
                setTours(data); // Устанавливаем полученные данные в состояние
            } catch (error) {
                console.error('Ошибка при загрузке данных:', error);
            }
        };

        fetchTours();
    }, []); // Пустой массив зависимостей, чтобы выполнить только один раз при монтировании

    const toggleFilters = () => {
        setIsFilterVisible(!isFilterVisible);
    };

    const handleReset = () => {
        setFilters(new FilterData());
    };

    const handleSearch = () => {
        // Здесь вы можете фильтровать данные на основе фильтров
        console.log(tours); // Выводим все туры
    };

    return (
        <div className="p-4">
            <h2 className="text-white text-4xl font-bold">Поиск по турам</h2>
            <input 
                type="text" 
                placeholder="Введите название тура" 
                className="mt-4 p-2 w-full rounded bg-gray-700 text-white"
                value={filters.search}
                onChange={(e) => setFilters({ ...filters, search: e.target.value })}
            />
            <button 
                onClick={toggleFilters} 
                className="mt-2 bg-blue-500 text-white px-4 py-2 rounded"
            >
                {isFilterVisible ? 'Скрыть фильтры' : 'Показать фильтры'}
            </button>

            {isFilterVisible && (
                <div className="mt-4 p-4 bg-gray-800 rounded">
                    {/* Ваши поля фильтров */}
                    <button 
                        onClick={handleReset} 
                        className="mt-2 bg-red-500 text-white px-4 py-2 rounded"
                    >
                        Сбросить филь
                        </button>
                </div>
            )}

            <button 
                onClick={handleSearch} 
                className="mt-4 bg-green-500 text-white px-4 py-2 rounded"
            >
                Найти туры
            </button>

            <div className="mt-4">
                <h3 className="text-white text-xl">Доступные туры</h3>
                <ul className="mt-2">
                    {tours.map(tour => (
                        <li key={tour.id} className="text-white">
                            <h4 className="font-bold">{tour.name}</h4>
                            <p>Дата: {tour.date}</p>
                            <p>Цена: {tour.price} руб.</p>
                            <p>Расстояние: {tour.distance} км</p>
                            <p>Рейтинг: {tour.rating}</p>
                            <p>Кредит: {tour.credit ? 'Да' : 'Нет'}</p>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}
