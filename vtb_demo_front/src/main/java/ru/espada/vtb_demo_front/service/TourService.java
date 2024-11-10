package ru.espada.vtb_demo_front.service;

import jakarta.annotation.PostConstruct;
import lombok.SneakyThrows;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.espada.vtb_demo_front.models.FilterData;
import ru.espada.vtb_demo_front.models.PayData;
import ru.espada.vtb_demo_front.models.ReviewData;
import ru.espada.vtb_demo_front.models.TourModel;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;

@Service
public class TourService {

    private List<TourModel> tours = new ArrayList<>();
    private int PAGE_SIZE = 3;
    @Autowired
    private ServiceService serviceService;

    @PostConstruct
    public void init() {
        for (int i = 0; i < 20; i++) {
            tours.add(getTour(i));
        }
    }

    @SneakyThrows
    public List<TourModel> getTours(FilterData filterData, int page) {
        Thread.sleep(3000);
        return tours.stream().filter(tour -> {
            if (filterData.getSearch() != null && !filterData.getSearch().isEmpty()) {
                return tour.getName().contains(filterData.getSearch());
            }
            return true;
        }).skip((long) PAGE_SIZE * page).limit(PAGE_SIZE).toList();
    }

    @SneakyThrows
    public boolean isFavorite(int id) {
        Thread.sleep(500);
        return new Random().nextBoolean();
    }

    public TourModel getTour(int id) {
        Random random = new Random();
        List<String> images = new ArrayList<>();
        images.add("https://avatars.mds.yandex.net/get-altay/248099/2a0000015e9f061499e2f143acd3dc7e6a77/XXL_height");
        images.add("https://vandruy.by/wp-content/uploads/2018/01/Pearl-2-min.jpg");
        List<ReviewData> reviews = new ArrayList<>();
        reviews.add(new ReviewData(0, "Тестовое имя", "https://avatars.mds.yandex.net/get-altay/248099/2a0000015e9f061499e2f143acd3dc7e6a77/XXL_height", "Тестовое описание", "%s.%s.%s".formatted(random.nextInt(30), random.nextInt(12), "20" + random.nextInt(24)), random.nextInt(5)));
        reviews.add(new ReviewData(1, "Тестовое имя 2", "https://vandruy.by/wp-content/uploads/2018/01/Pearl-2-min.jpg", "Тестовое описание 2 длинное текст чтобы вылезало на экран и можно было проверить выравнивание текста", "%s.%s.%s".formatted(random.nextInt(30), random.nextInt(12), "20" + random.nextInt(24)), random.nextInt(5)));
        reviews.add(new ReviewData(2, "Тестовое имя 3", "https://avatars.mds.yandex.net/get-altay/248099/2a0000015e9f061499e2f143acd3dc7e6a77/XXL_height", "Тестовое описание", "01.01.2022", random.nextInt(5)));

        List<PayData> payVariants = new ArrayList<>();
        payVariants.add(new PayData("Со страхованием", List.of("Сразу - " + random.nextInt(100000) + " руб.", "В кредит - " + random.nextInt(100000) + " руб.")));
        payVariants.add(new PayData("Без страхования", List.of("Сразу - " + random.nextInt(100000) + " руб.", "В кредит - " + random.nextInt(100000) + " руб.")));

        return new TourModel(
                id,
                "Тур " + random.nextInt(100),
                random.nextBoolean() ? images.get(0) : images.get(1),
                images,
                "Тестовое описание " + random.nextInt(100),
                "Тестовый адрес " + random.nextInt(100),
                "Тестовое описание " + random.nextInt(100),
                List.of("Тестовая категория " + random.nextInt(100), "Тестовая категория " + random.nextInt(100)),
                random.nextInt(10000),
                reviews,
                5,
                payVariants
        );
    }

    @SneakyThrows
    public List<TourModel> getFavorites() {
        Thread.sleep(200);
        return tours.stream().filter(tour -> tour.getId() % 3 == 0).toList();
    }
}
