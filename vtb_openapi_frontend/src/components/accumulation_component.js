"use client";
import useAccumulationStore from "../stores/accumulation_store.js";

export default function AccumulationComponent() {
    const { tourName, sum, actualSum } = useAccumulationStore();

    const percentage = Math.min((actualSum / sum) * 100, 100);

    return (
        <div className="bg-custom-blur min-h-96 px-5">
            <div className="container py-24 text-white text-3xl font-semibold">
                <h3 className="text-white md:mb-20 md:text-3xl lg:text-5xl font-bold">На тур в {tourName} вместе с ВТБ</h3>
                
                <div className="w-full bg-custom-bg-gray rounded-md overflow-hidden my-10">
                    <div
                        className="bg-custom-gradient h-10 transition-all duration-300"
                        style={{ width: `${percentage}%` }}
                    >
                        <span className="absolute left-1/2 transform -translate-x-1/2 text-white font-bold">
                            {Math.round(percentage)}%
                        </span>
                    </div>
                </div>
                <p>{actualSum}/{sum}₽</p>          
            </div>
        </div>
    );
}
