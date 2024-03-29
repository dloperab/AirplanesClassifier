# import the necessary packages
from requests import exceptions
import requests
import cv2
import os

class SearchBingApi:
    @staticmethod
    def search(query, output, maxResults, groupSize):
        # set your Microsoft Cognitive Services API key
        # API_KEY = "<YOUR_API_KEY_GOES_HERE>"

        # set the endpoint API URL
        URL = "https://api.cognitive.microsoft.com/bing/v7.0/images/search"

        # when attempting to download images from the web both the Python
        # programming language and the requests library have a number of
        # exceptions that can be thrown so let's build a list of them now
        # so we can filter on them
        EXCEPTIONS = set([IOError, FileNotFoundError,
            exceptions.RequestException, exceptions.HTTPError,
            exceptions.ConnectionError, exceptions.Timeout])

        # store the search term in a convenience variable then set the
        # headers and search parameters
        term = query
        headers = {"Ocp-Apim-Subscription-Key" : API_KEY}
        params = {"q": term, "offset": 0, "count": groupSize}

        # make the search
        print("[INFO] searching Bing API for '{}'".format(term))
        search = requests.get(URL, headers=headers, params=params)
        search.raise_for_status()

        # grab the results from the search, including the total number of
        # estimated results returned by the Bing API
        results = search.json()
        estNumResults = min(results["totalEstimatedMatches"], maxResults)
        print("[INFO] {} total results for '{}'".format(estNumResults, term))

        # initialize the total number of images downloaded thus far
        total = 0

        # loop over the estimated number of results in 'groupSize' groups
        for offset in range(0, estNumResults, groupSize):
            # update the search parameters using the current offset, then
            # make the request to fetch the results
            print("[INFO] making request for group {}-{} of {}...".format(
                offset, offset + groupSize, estNumResults))
            params["offset"] = offset
            search = requests.get(URL, headers=headers, params=params)
            search.raise_for_status()
            results = search.json()
            print("[INFO] saving images for group {}-{} of {}...".format(
                offset, offset + groupSize, estNumResults))

            # loop over the results
            for v in results["value"]:
                # try to download the image
                try:
                    # make a request to download the image
                    print("[INFO] fetching: {}".format(v["contentUrl"]))
                    r = requests.get(v["contentUrl"], timeout=30)

                    # build the path to the output image
                    ext = v["contentUrl"][v["contentUrl"].rfind("."):]
                    p = os.path.sep.join([output, "{}{}".format(
                        str(total).zfill(3), ext)])

                    # write the image to disk
                    f = open(p, "wb")
                    f.write(r.content)
                    f.close()
                except Exception as e:
                    # catch any errors that would not unable us to download the image
                    # check to see if our exception is in our list of
                    # exceptions to check for
                    if type(e) in EXCEPTIONS:
                        print("[INFO] skipping: {}".format(v["contentUrl"]))
                        continue

                # try to load the image from disk
                image = cv2.imread(p)

                # if the image is `None` then we could not properly load the
                # image from disk (so it should be ignored)
                if image is None:
                    print("[INFO] deleting: {}".format(p))
                    os.remove(p)
                    continue

                # update the counter
                total += 1
