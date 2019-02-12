# import the necessary packages
from search_bing_api import SearchBingApi

# set global params
MAX_RESULTS = 150
GROUP_SIZE = 50

# set params for Airbus search
query = "airbus"
output = "../../../dataset/airbus"
print("[INFO] downloading images for Airbus")
SearchBingApi.search(query, output, MAX_RESULTS, GROUP_SIZE)

# set params for Boeing search
query = "boeing"
output = "../../../dataset/boeing"
print("[INFO] downloading images for Boeing")
SearchBingApi.search(query, output, MAX_RESULTS, GROUP_SIZE)
