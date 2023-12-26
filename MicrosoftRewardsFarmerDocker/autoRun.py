import main, sys, schedule, random, time, datetime

def job():
    print("Starting at " + datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S") + "...")
    main.sys.argv[1:] = sys.argv[1:]
    
    try:
        main.main()
    except Exception as e:
        print(e)
        print("Error occured, restarting...")
        job()

def run_at_time():
    random_minute = random.randint(0, 59)

    if random_minute <= 9:
        run_time = f"03:0{random_minute}"
    else:
        run_time = f"03:{random_minute}"
    schedule.every().day.at(run_time).do(job)
    next_run_time = datetime.datetime.now() + datetime.timedelta(days=1)
    schedule.every().day.at("04:00").do(run_at_time).tag('reschedule')
    print(f"Job scheduled for {next_run_time.strftime('%Y-%m-%d')} at {run_time}")

if __name__ == "__main__":
    job() # run once at start
    run_at_time()

    while True:
        schedule.run_pending()
        time.sleep(1)