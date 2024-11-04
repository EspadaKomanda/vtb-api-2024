// TourItem.jsx


const TourItem = ({ tour }) => {
    return (
        <li className="relative h-96 flex flex-col justify-end rounded mb-10">
            <div
                className="absolute inset-0 bg-cover bg-center rounded"
                style={{ backgroundImage: `url(${tour.image})` }}
            >
                <div className="absolute inset-0 bg-customColor1 opacity-50 rounded"></div>
            </div>
            <div className="relative min-h-1/3 h-auto p-4 backdrop-blur-md rounded"> 
                <h4 className="text-white text-lg font-bold">{tour.name}</h4>
                <p className="text-gray-300">Описание: {tour.description}</p>
                <p className="text-gray-300">Цена: {tour.price} ₽</p>
                <p className="text-gray-300">Рейтинг: {tour.rating}</p>
            </div>
        </li>
    );
};

export default TourItem;
