"use client";
import { useState } from "react"
import useAccumulationStore from "../stores/accumulation_store.js"

export default function AccumulationComponent() {

    const { tourName, setTourName, sum, setSum, actualSum, setActualSum } = useAccumulationStore();

    return (
        <div className="bg-custom-blur min-h-96">
            <div className="conteiner py-24">
                  <h3 className="text-white text-5xl font-bold">На тур в {tourName} вместе с ВТБ</h3>          
            </div>
        </div>
    )
}
