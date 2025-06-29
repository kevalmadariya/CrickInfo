from fastapi import FastAPI
from scrape import scrape_points_and_schedule
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Allow all origins for CORS (you can restrict to your frontend later)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/scrape-data")
def get_scraped_data():
    points_data, schedule_data, orange_cap_data, purple_cap_data = scrape_points_and_schedule()
    return {
        "points_table": points_data,
        "schedule": schedule_data,
        "orange_cap": orange_cap_data,
        "purple_cap": purple_cap_data
    }
