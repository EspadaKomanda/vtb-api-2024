"use client";
import { useEffect, useState } from 'react';
import Image from 'next/image';
import * as img from '../assets/images.js';
import Slider from 'rc-slider';
import 'rc-slider/assets/index.css';
import '../styles/saerch_tour_component_styles.css';
import StarRating from './star_rating.js';

class FilterData {
    constructor() {
        this.search = '';
        this.tours = false;
        this.entertainment = false;
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
                const response = await fetch('/tours.json');
                if (!response.ok) {
                    throw new Error('Сеть не отвечает');
                }
                const data = await response.json();
                setTours(data);
            } catch (error) {
                console.error('Ошибка при загрузке данных:', error);
            }
        };

        fetchTours();
    }, []);

    const toggleFilters = () => {
        setIsFilterVisible(!isFilterVisible);
    };

    const handleReset = () => {
        setFilters(new FilterData());
    };

    const handleSearch = (e) => {
        e.preventDefault();
        console.log(filters);
    };

    const handleDistanceChange = (value) => {
        setFilters({ ...filters, distanceFrom: value[0], distanceTo: value[1] });
    };

    const handlePriceChange = (value) => {
        setFilters({ ...filters, priceFrom: value[0], priceTo: value[1] });
    };

    return (
        <div className="p-4">
            <h2 className="text-white text-4xl font-bold">Поиск по турам и развлечениям</h2>
            <form onSubmit={handleSearch} className='relative'>
                <input 
                    type="text" 
                    placeholder="Введите название тура" 
                    className="mt-4 p-2 w-full rounded bg-custom-bg-gray"
                    value={filters.search}
                    onChange={(e) => setFilters({ ...filters, search: e.target.value })}
                />
                <button 
                    type="button" 
                    onClick={toggleFilters} 
                    className="sm:right-28 right-20 top-6 absolute"
                >
                    <Image src={img.filter} alt="filter" width={25} height={25} className='ml-auto' />
                </button>
                
                {isFilterVisible && (
                    <div className="right-3 mt-4 px-8 py-2 bg-custom-bg-gray rounded lg:w-1/2 flex flex-col gap-y-2 absolute text-2xl">
                        <div className="flex justify-end items-center">
                            <Image 
                                src={img.exit} 
                                alt="close" 
                                width={30}
                                height={30} 
                                onClick={toggleFilters} 
                                className='' 
                            />
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Туры:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="tours" 
                                    className="hidden" 
                                    checked={filters.tours} 
                                    onChange={(e) => setFilters({ ...filters, tours: e.target.checked })}
                                />
                                <label 
                                    htmlFor="tours" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm 
                                        ${filters.tours ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.tours && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Развлечения:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="entertainment" 
                                    className="hidden" 
                                    checked={filters.entertainment} 
                                    onChange={(e) => setFilters({ ...filters, entertainment: e.target.checked })}
                                />
                                <label 
                                    htmlFor="entertainment" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm 
                                        ${filters.entertainment ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.entertainment && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Дата от:</label>
                            <input 
                                type="date" 
                                className="mt-1 px-2 py-1 rounded bg-custom-blur text-white"
                                value={filters.dateFrom || ''}
                                onChange={(e) => setFilters({ ...filters, dateFrom: e.target.value })}
                            />
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Дата до:</label>
                            <input 
                                type="date" 
                                className="mt-1 px-2 py-1 rounded bg-custom-blur text-white"
                                value={filters.dateTo || ''}
                                onChange={(e) => setFilters({ ...filters, dateTo: e.target.value })}
                            />
                        </div>
                        <div className='grid grid-cols-2 '>
                            <label className="text-white">Стоимость:</label>
                            <div className="flex flex-col">
                                <Slider
                                    range
                                    min={0}
                                    max={100000}
                                    value={[filters.priceFrom || 10000, filters.priceTo || 50000]}
                                    onChange={handlePriceChange}
                                    className="my-2 custom-slider"
                                />
                                <div className="flex justify-between">
                                    <input 
                                        type="number" 
                                        className="mt-1 mr-5 px-2 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.priceFrom || 10000}
                                        onChange={(e) => setFilters({ ...filters, priceFrom: e.target.value })}
                                    />
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.priceTo || 50000}
                                        onChange={(e) => setFilters({ ...filters, priceTo: e.target.value })}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className='grid grid-cols-2 '>
                            <label className="text-white">Расстояние:</label>
                            <div className="flex flex-col">
                                <Slider
                                    range
                                    min={0}
                                    max={100000}
                                    value={[filters.distanceFrom || 10000, filters.distanceTo || 50000]}
                                    onChange={handleDistanceChange}
                                    className="my-2 custom-slider"
                                />
                                <div className="flex justify-between">
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2 mr-5 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.distanceFrom || 10000}
                                        onChange={(e) => setFilters({ ...filters, distanceFrom: e.target.value })}
                                    />
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2  rounded bg-custom-blur text-white w-1/2"
                                        value={filters.distanceTo || 50000}
                                        onChange={(e) => setFilters({ ...filters, distanceTo: e.target.value })}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Рейтинг от:</label>
                            <StarRating 
                                rating={filters.ratingFrom} 
                                setRating={(value) => setFilters({ ...filters, ratingFrom: value })} 
                            />
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Кредит:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="credit" 
                                    className="hidden" 
                                    checked={filters.credit} 
                                    onChange={(e) => setFilters({ ...filters, credit: e.target.checked })}
                                />
                                <label 
                                    htmlFor="credit" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm 
                                        ${filters.credit ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.credit && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Типы:</label>
                            <select 
                                multiple 
                                className="mt-1 p-2 rounded bg-custom-blur text-white"
                                value={filters.types}
                                onChange={(e) => {
                                    const options = Array.from(e.target.selectedOptions, option => option.value);
                                    setFilters({ ...filters, types: options });
                                }}
                            >
                                <option value="excursion">Экскурсии</option>
                                <option value="adventure">Приключения</option>
                                <option value="cultural">Культурные</option>
                                <option value="relaxation">Релаксация</option>
                                <option value="family">Семейные</option>
                                <option value="romantic">Романтические</option>
                            </select>
                        </div>

                        <button 
                            type="button" 
                            onClick={handleReset} 
                            className="mt-2 bg-custom-blur text-white px-4 py-2 rounded text-3xl"
                        >
                            Сбросить
                        </button>
                    </div>
                )}

                <button 
                    type="submit" 
                    className="bg-custom-gradient text-white py-1 px-2 sm:px-5 rounded right-1 top-5 absolute"
                >
                    Найти
                </button>
            </form>

            <div className="mt-4">
                <h3 className="text-white text-xl">Доступные туры и развлечения</h3>
                <ul className="mt-2">
                    {tours.map((tour, index) => (
                        <li key={index} className="p-4 rounded mb-2">
                            <h4 className="text-white text-lg font-bold">{tour.name}</h4>
                            <p className="text-gray-300">Описание: {tour.description}</p>
                            <p className="text-gray-300">Цена: {tour.price} ₽</p>
                            <p className="text-gray-300">Рейтинг: {tour.rating}</p>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

